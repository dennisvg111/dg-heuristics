using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Graphs.Tsp
{
    public class BranchAndBoundTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData> where TData : IGraphPoint<TData>
    {
        private readonly List<TData> _points;
        private double[,] _distanceMatrix;

        public BranchAndBoundTraveler(IEnumerable<TData> points)
        {
            _points = points.ToList();
        }

        public BranchAndBoundTraveler() : this(new List<TData>())
        {
        }

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
            if (_distanceMatrix != null)
            {
                return;
            }

            _distanceMatrix = new double[_points.Count, _points.Count];

            for (int i = 0; i < _points.Count; i++)
            {
                for (int j = i + 1; j < _points.Count; j++)
                {
                    double distance = _points[i].DistanceTo(_points[j]);
                    _distanceMatrix[i, j] = distance;
                    _distanceMatrix[j, i] = distance; // symmetric TSP
                }
            }
        }

        public List<TData> CalculateRoute(out double totalDistance)
        {
            BuildDistanceMatrixIfNeeded();
            var results = new IntermediateResult(_points.Count);
            ExplorePaths(0, 0, results);

            totalDistance = results.BestTotalDistance;
            return results.BestRouteIndices?.Select(i => _points[i]).ToList() ?? new List<TData>();
        }

        /// <summary>
        /// Recursively explores paths using branch and bound with precomputed distances.
        /// </summary>
        private void ExplorePaths(double accumulatedDistance, int depth, IntermediateResult results)
        {
            if (depth == _points.Count)
            {
                if (accumulatedDistance < results.BestTotalDistance)
                {
                    results.BestTotalDistance = accumulatedDistance;

                    results.BestRouteIndices = new int[_points.Count];
                    Array.Copy(results.CurrentRoute, results.BestRouteIndices, _points.Count);
                }
                return;
            }

            for (int i = 0; i < _points.Count; i++)
            {
                if (results.Visited[i]) continue;

                int lastIndex = depth > 0 ? results.CurrentRoute[depth - 1] : -1;
                double stepDistance = lastIndex >= 0 ? _distanceMatrix[lastIndex, i] : 0;

                if (accumulatedDistance + stepDistance >= results.BestTotalDistance)
                    continue;

                results.Visited[i] = true;
                results.CurrentRoute[depth] = i;

                ExplorePaths(accumulatedDistance + stepDistance, depth + 1, results);

                results.Visited[i] = false;
            }
        }

        private class IntermediateResult
        {
            public int[] CurrentRoute { get; }
            public bool[] Visited { get; }
            public double BestTotalDistance { get; set; }
            public int[] BestRouteIndices { get; set; }

            public IntermediateResult(int count)
            {
                CurrentRoute = new int[count];
                Visited = new bool[count];
                BestTotalDistance = double.MaxValue;
                BestRouteIndices = null;
            }
        }
    }
}
