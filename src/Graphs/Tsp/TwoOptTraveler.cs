using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Graphs.Tsp
{
    public class TwoOptTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData> where TData : IGraphPoint<TData>
    {
        private readonly List<TData> _points;
        private CachedDistanceMatrix<TData> _cache;

        public TwoOptTraveler(IEnumerable<TData> points)
        {
            _points = points.ToList();
            _cache = new CachedDistanceMatrix<TData>(_points);
        }

        public TwoOptTraveler() : this(new List<TData>())
        {
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

        private int[] GetNearestNeighbourRoute()
        {
            var route = new int[_points.Count];
            var unvisited = new HashSet<int>(Enumerable.Range(1, route.Length - 1));
            var current = 0;
            route[0] = current;
            for (int i = 1; i < route.Length; i++)
            {
                var nearest = unvisited
                    .OrderBy(p => _cache.CalculateDistanceBetween(current, p))
                    .First();

                current = nearest;
                route[i] = current;
                unvisited.Remove(current);
            }
            return route;
        }

        public List<TData> CalculateRoute(out double totalDistance)
        {
            var route = GetNearestNeighbourRoute();
            var currentDistance = _cache.CalculateTotalDistance(route);
            bool improved = true;

            while (improved)
            {
                improved = false;

                for (int i = 0; i < route.Length - 1 && !improved; i++)
                {
                    for (int k = i + 1; k < route.Length && !improved; k++)
                    {
                        var delta = CalculateDelta(route, i, k);
                        if (delta < 0)
                        {
                            var newRoute = TwoOptSwap(route, i, k);
                            route = newRoute;
                            currentDistance += delta;
                            improved = true;
                        }
                    }
                }
            }

            totalDistance = currentDistance;
            return route.Select(i => _points[i]).ToList();
        }

        private double CalculateDelta(int[] route, int i, int k)
        {
            int n = route.Length;

            int i_prev = i - 1;
            int k_next = k + 1;

            double oldDistance = 0;
            double newDistance = 0;

            if (i_prev >= 0)
            {
                oldDistance += _cache.CalculateDistanceBetween(route[i_prev], route[i]);
                newDistance += _cache.CalculateDistanceBetween(route[i_prev], route[k]);
            }

            if (k_next < n)
            {
                oldDistance += _cache.CalculateDistanceBetween(route[k], route[k_next]);
                newDistance += _cache.CalculateDistanceBetween(route[i], route[k_next]);
            }

            return newDistance - oldDistance;
        }

        private int[] TwoOptSwap(int[] route, int i, int k)
        {
            int length = route.Length;
            int[] newRoute = new int[length];

            // Copy the segment before i
            Array.Copy(route, 0, newRoute, 0, i);

            // Reverse the segment from i to k
            for (int j = 0; j <= k - i; j++)
            {
                newRoute[i + j] = route[k - j];
            }

            // Copy the segment after k
            Array.Copy(route, k + 1, newRoute, k + 1, length - k - 1);

            return newRoute;
        }
    }
}
