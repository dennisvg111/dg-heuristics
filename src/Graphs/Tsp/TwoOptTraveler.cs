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

        public List<TData> CalculateRoute(out double totalDistance)
        {
            // Start with the input order
            var route = Enumerable.Range(0, _points.Count).ToArray();
            var currentDistance = _cache.CalculateTotalDistance(route);
            bool improved = true;

            while (improved)
            {
                improved = false;

                for (int i = 0; i < route.Length - 1; i++)
                {
                    for (int k = i + 1; k < route.Length; k++)
                    {
                        var newRoute = TwoOptSwap(route, i, k);
                        var newDistance = _cache.CalculateTotalDistance(newRoute);
                        if (newDistance < currentDistance)
                        {
                            route = newRoute;
                            currentDistance = newDistance;
                            improved = true;
                        }
                    }
                }
            }

            totalDistance = currentDistance;
            return route.Select(i => _points[i]).ToList();
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
