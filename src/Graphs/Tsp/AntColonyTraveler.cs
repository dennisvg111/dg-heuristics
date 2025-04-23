using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Heuristic.Graphs.Tsp
{
    public class AntColonyTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData> where TData : IGraphPoint<TData>
    {
        private readonly AntColonyOptions _options;

        private readonly List<TData> _points = new List<TData>();
        private CachedDistanceMatrix<TData> _cache;

        private double[,] _pheromones;

        public AntColonyTraveler(AntColonyOptions options)
        {
            _cache = new CachedDistanceMatrix<TData>(_points);
            _options = options;
        }

        public AntColonyTraveler() : this(AntColonyOptions.Default)
        {
        }

        /// <inheritdoc/>
        public bool Add(TData item)
        {
            _points.Add(item);
            _cache.SetPoints(_points);
            return true;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _points.Clear();
            _cache.SetPoints(_points);
        }

        /// <inheritdoc/>
        public List<TData> CalculateRoute(out double totalDistance)
        {
            int n = _points.Count;
            _pheromones = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    _pheromones[i, j] = 1.0;

            var ants = Enumerable.Range(0, _options.AntCount).Select(i => new Ant(n)).ToArray();
            totalDistance = double.MaxValue;
            int stagnationCount = 0;

            for (int iteration = 0; iteration < _options.Iterations; iteration++)
            {
                Parallel.ForEach(ants, ant =>
                {
                    var distance = ConstructRoute(ant);
                    if (ant.UpdateRouteIfBetter(distance))
                    {
                        DepositPheromones(ant.CurrentRoute, distance);
                    }
                });

                var foundDistance = ants.Min(a => a.BestDistance);
                stagnationCount = foundDistance < totalDistance ? 0 : (stagnationCount + 1);
                totalDistance = foundDistance;
                if (stagnationCount > _options.StagnationMax)
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
            int usedBufferSize = 0;
            var current = ant.Current;

            for (int city = 1; city <= ant.BufferSize; city++)
            {
                if (ant.HasVisited(city))
                {
                    continue;
                }

                double pheromone = Math.Pow(_pheromones[current, city], _options.Alpha);
                var distanceToCity = _cache.CalculateDistanceBetween(current, city);
                double visibility = Math.Pow(1.0 / (distanceToCity + 1e-6), _options.Beta);
                double score = pheromone * visibility;
                ant.UpdateBuffer(usedBufferSize, city, distanceToCity, score);
                usedBufferSize++;
            }

            return ant.PickCityFromBuffer(usedBufferSize, out distance);
        }

        private void EvaporatePheromones()
        {
            int n = _points.Count;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    _pheromones[i, j] *= (1.0 - _options.EvaporationRate);
        }

        private void DepositPheromones(IReadOnlyList<int> route, double distance)
        {
            double deposit = _options.Q / distance;
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
            private bool[] _visited;
            private int _unvisitedCount;

            private readonly PointProbability[] _optionsBuffer;
            private readonly double[] _cumulativeScoreBuffer;
            private double _bufferScoreSum = 0;

            private double _bestDistance;
            private int[] _bestRoute;

            public double BestDistance => _bestDistance;

            public IReadOnlyCollection<int> BestRoute => _bestRoute;

            public int Current => _currentRoute[_currentStep - 1];
            public IReadOnlyList<int> CurrentRoute => _currentRoute;
            public bool HasUnvisited => _unvisitedCount > 0;

            public int BufferSize => _pointsCount - 1;

            public Ant(int pointsCount)
            {
                _pointsCount = pointsCount;

                _currentRoute = new int[_pointsCount];

                _bestRoute = new int[_pointsCount];
                _bestDistance = double.MaxValue;
                _visited = new bool[pointsCount];

                _random = new Random();
                _optionsBuffer = new PointProbability[pointsCount - 1];
                _cumulativeScoreBuffer = new double[pointsCount - 1];
                for (int i = 0; i < _optionsBuffer.Length; i++)
                {
                    _optionsBuffer[i] = PointProbability.Zero;
                }
            }

            public void UpdateBuffer(int bufferIndex, int city, double distance, double score)
            {
                if (bufferIndex == 0)
                {
                    _bufferScoreSum = 0;
                }
                _bufferScoreSum += score;
                _cumulativeScoreBuffer[bufferIndex] = _bufferScoreSum;
                _optionsBuffer[bufferIndex].Update(city, distance, score);
            }

            public int PickCityFromBuffer(int usedBufferSize, out double distance)
            {
                var index = PickBufferIndex(usedBufferSize);
                distance = _optionsBuffer[index].Distance;
                return _optionsBuffer[index].Index;
            }

            private int PickBufferIndex(int usedBufferSize)
            {
                if (usedBufferSize == 1)
                {
                    return 0;
                }

                double threshold = _random.NextDouble() * _bufferScoreSum;

                for (int i = 0; i < usedBufferSize; i++)
                {
                    if (_cumulativeScoreBuffer[i] >= threshold)
                    {
                        return i;
                    }
                }

                return 0;
            }

            public void Reset()
            {
                _currentStep = 0;
                _unvisitedCount = _pointsCount;
                Array.Clear(_visited, 0, _visited.Length);
            }

            public bool HasVisited(int i) => _visited[i];

            public void Visit(int i)
            {
                _visited[i] = true;
                _unvisitedCount--;
                _currentRoute[_currentStep] = i;
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
    }
}
