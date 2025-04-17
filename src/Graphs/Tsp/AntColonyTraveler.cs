using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Heuristic.Graphs.Tsp
{
    public class AntColonyTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData> where TData : IGraphPoint<TData>
    {
        private const int AntCount = 30;
        private const int Alpha = 1; // Influence of pheromone
        private const int Beta = 5; // Influence of distance
        private const double EvaporationRate = 0.5;
        private const double Q = 100.0; // Constant used to deposit pheromones
        private const int Iterations = 100;
        private const int StagnationMax = 25;

        private readonly List<TData> _points = new List<TData>();
        private CachedDistanceMatrix<TData> _cache;

        private double[,] _pheromones;

        public AntColonyTraveler()
        {
            _cache = new CachedDistanceMatrix<TData>(_points);
        }

        public bool Add(TData item)
        {
            _points.Add(item);
            _cache.SetPoints(_points);
            return true;
        }

        public void Clear()
        {
            _points.Clear();
            _cache.SetPoints(_points);
        }

        public List<TData> CalculateRoute(out double totalDistance)
        {
            int n = _points.Count;
            _pheromones = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    _pheromones[i, j] = 1.0;

            var ants = Enumerable.Range(0, AntCount).Select(i => new Ant(n)).ToArray();
            totalDistance = double.MaxValue;
            int stagnationCount = 0;

            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                foreach (var ant in ants)
                {
                    var distance = ConstructRoute(ant);
                    if (ant.UpdateRouteIfBetter(distance))
                    {
                        DepositPheromones(ant.CurrentRoute, distance);
                    }
                }
                Parallel.ForEach(ants, ant =>
                {
                });

                var foundDistance = ants.Min(a => a.BestDistance);
                stagnationCount = foundDistance < totalDistance ? 0 : (stagnationCount + 1);
                totalDistance = foundDistance;
                if (stagnationCount > StagnationMax)
                {
                    break;
                }

                EvaporatePheromones();
            }

            var bestAnt = ants.OrderBy(a => a.BestDistance).First();
            return bestAnt.BestRoute.Select(i => _points[i]).ToList();
        }

        private double ConstructRoute(Ant ant)
        {
            ant.Reset();
            ant.Visit(0);
            double distance = 0;

            while (ant.HasUnvisited)
            {
                int next = SelectNextCity(ant, out double stepDistance);
                distance += stepDistance;
                ant.Visit(next);
            }

            return distance;
        }

        private int SelectNextCity(Ant ant, out double distance)
        {
            double sum = 0;
            int bufferSize = 0;
            var current = ant.Current;

            var buffer = ant.OptionsBuffer;

            for (int city = 1; city <= buffer.Count; city++)
            {
                if (ant.IsVisited(city))
                {
                    continue;
                }

                double pheromone = FastPow(_pheromones[current, city], Alpha);
                var distanceToCity = _cache.CalculateDistanceBetween(current, city);
                double visibility = FastPow(1.0 / (distanceToCity + 1e-6), Beta);
                double score = pheromone * visibility;
                buffer[bufferSize].Update(city, score, distanceToCity);
                bufferSize++;
                sum += score;
            }

            double threshold = ant.NextDouble() * sum;
            double cumulative = 0;
            for (int j = 0; j < bufferSize; j++)
            {
                cumulative += buffer[j].Score;
                if (cumulative >= threshold)
                {
                    distance = buffer[j].Distance;
                    return buffer[j].City;
                }
            }

            // Fallback
            distance = buffer[0].Distance;
            return buffer[0].City;
        }

        private double FastPow(double num, int exp)
        {
            double result = 1.0;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= num;
                exp >>= 1;
                num *= num;
            }

            return result;
        }

        private void EvaporatePheromones()
        {
            int n = _points.Count;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    _pheromones[i, j] *= (1.0 - EvaporationRate);
        }

        private void DepositPheromones(IReadOnlyList<int> route, double distance)
        {
            double deposit = Q / distance;
            for (int i = 0; i < route.Count - 1; i++)
            {
                int from = route[i];
                int to = route[i + 1];
                _pheromones[from, to] += deposit;
                _pheromones[to, from] += deposit;
            }
        }

        private class Ant
        {
            private int _currentStep;
            private readonly int[] _currentRoute;

            private readonly int _pointsCount;
            private readonly Random _random;
            private readonly CityProbability[] _optionsBuffer;
            private bool[] _visited;
            private int _toVisit = 0;

            private double _bestDistance;
            private int[] _bestRoute;

            public double BestDistance => _bestDistance;

            public IReadOnlyCollection<int> BestRoute => _bestRoute;

            public IReadOnlyList<CityProbability> OptionsBuffer => _optionsBuffer;

            public bool HasUnvisited => _toVisit > 0;

            public int Current => _currentRoute[_currentStep - 1];
            public IReadOnlyList<int> CurrentRoute => _currentRoute;

            public Ant(int pointsCount)
            {
                _pointsCount = pointsCount;

                _currentRoute = new int[_pointsCount];

                _bestRoute = new int[_pointsCount];
                _bestDistance = double.MaxValue;
                _visited = new bool[pointsCount];
                _toVisit = _pointsCount;

                _random = new Random();
                _optionsBuffer = new CityProbability[pointsCount - 1];
                for (int i = 0; i < _optionsBuffer.Length; i++)
                {
                    _optionsBuffer[i] = new CityProbability(0, 0, 0);
                }
            }

            public double NextDouble()
            {
                return _random.NextDouble();
            }

            public bool IsVisited(int i)
            {
                return _visited[i];
            }

            public void Reset()
            {
                _currentStep = 0;
                _visited = new bool[_pointsCount];
                _toVisit = _pointsCount;
            }

            public void Visit(int i)
            {
                _currentRoute[_currentStep] = i;
                _visited[i] = true;
                _toVisit--;
                _currentStep++;
            }

            public bool UpdateRouteIfBetter(double distance)
            {
                if (distance > _bestDistance)
                {
                    return false;
                }
                _bestDistance = distance;
                Array.Copy(_currentRoute, _bestRoute, _pointsCount);
                return true;
            }
        }

        private class CityProbability
        {
            public int City { get; set; }
            public double Score { get; set; }
            public double Distance { get; set; }

            public CityProbability(int city, double score, double distance)
            {
                City = city;
                Score = score;
                Distance = distance;
            }

            public void Update(int city, double score, double distance)
            {
                City = city;
                Score = score;
                Distance = distance;
            }

            public override string ToString()
            {
                return $"[{City}] {Distance}, score: {Score}";
            }
        }
    }
}
