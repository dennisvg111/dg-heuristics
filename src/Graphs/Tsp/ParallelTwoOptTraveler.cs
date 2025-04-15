using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Heuristic.Graphs.Tsp
{
    public class ParallelTwoOptTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData>
        where TData : IGraphPoint<TData>
    {
        private List<TData> _points;
        private double[,] _distanceMatrix;

        public ParallelTwoOptTraveler(IEnumerable<TData> points)
        {
            _points = points.ToList();
            BuildDistanceMatrixIfNeeded();
        }

        public ParallelTwoOptTraveler() : this(new List<TData>()) { }

        public bool Add(TData item)
        {
            _points.Add(item);
            _distanceMatrix = null;
            return true;
        }

        public void Clear()
        {
            _points.Clear();
            _distanceMatrix = null;
        }

        private void BuildDistanceMatrixIfNeeded()
        {
            if (_distanceMatrix != null) return;

            int count = _points.Count;
            _distanceMatrix = new double[count, count];
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    double distance = _points[i].DistanceTo(_points[j]);
                    _distanceMatrix[i, j] = distance;
                    _distanceMatrix[j, i] = distance;
                }
            }
        }

        public List<TData> CalculateRoute(out double totalDistance)
        {
            BuildDistanceMatrixIfNeeded();

            int n = _points.Count;
            int[] route = Enumerable.Range(0, n).ToArray();
            double bestDistance = CalculateTotalDistance(route);
            bool improved = true;

            while (improved)
            {
                improved = false;
                double bestDelta = 0;
                int bestI = -1, bestK = -1;
                object locker = new object();

                Parallel.For(1, n - 1, i =>
                {
                    for (int k = i + 1; k < n; k++)
                    {
                        double delta = CalculateDelta(route, i, k);
                        if (delta < bestDelta)
                        {
                            lock (locker)
                            {
                                if (delta < bestDelta)
                                {
                                    bestDelta = delta;
                                    bestI = i;
                                    bestK = k;
                                }
                            }
                        }
                    }
                });

                if (bestDelta < 0)
                {
                    route = TwoOptSwap(route, bestI, bestK);
                    bestDistance += bestDelta;
                    improved = true;
                }
            }

            totalDistance = bestDistance;
            return route.Select(i => _points[i]).ToList();
        }

        private double CalculateDelta(int[] route, int i, int k)
        {
            int a = route[i - 1];
            int b = route[i];
            int c = route[k];
            int d = k + 1 < route.Length ? route[k + 1] : -1;

            double removed = _distanceMatrix[a, b] + (d != -1 ? _distanceMatrix[c, d] : 0);
            double added = _distanceMatrix[a, c] + (d != -1 ? _distanceMatrix[b, d] : 0);
            return added - removed;
        }

        private int[] TwoOptSwap(int[] route, int i, int k)
        {
            int[] newRoute = new int[route.Length];

            // Copy up to i-1
            Array.Copy(route, 0, newRoute, 0, i);

            // Reverse segment from i to k
            for (int j = 0; j <= k - i; j++)
            {
                newRoute[i + j] = route[k - j];
            }

            // Copy the rest
            Array.Copy(route, k + 1, newRoute, k + 1, route.Length - k - 1);
            return newRoute;
        }

        private double CalculateTotalDistance(int[] route)
        {
            double dist = 0;
            for (int i = 1; i < route.Length; i++)
            {
                dist += _distanceMatrix[route[i - 1], route[i]];
            }
            return dist;
        }
    }
}
