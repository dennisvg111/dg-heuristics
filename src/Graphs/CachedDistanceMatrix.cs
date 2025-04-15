using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DG.Heuristic.Graphs
{
    public class CachedDistanceMatrix<TPoint> where TPoint : IGraphPoint<TPoint>
    {
        private Lazy<double[,]> _matrix;
        private IReadOnlyList<TPoint> _points;

        public CachedDistanceMatrix()
        {
            Reset();
        }

        public void Reset()
        {
            _matrix = new Lazy<double[,]>(() => BuildDistanceMatrixIfNeeded());
        }

        public void SetPoints(IReadOnlyList<TPoint> data)
        {
            _points = data;
            Reset();
        }

        private double[,] BuildDistanceMatrixIfNeeded()
        {
            var distanceMatrix = new double[_points.Count, _points.Count];

            for (int i = 0; i < _points.Count; i++)
            {
                for (int j = i + 1; j < _points.Count; j++)
                {
                    double distance = _points[i].DistanceTo(_points[j]);
                    distanceMatrix[i, j] = distance;
                    distanceMatrix[j, i] = distance; // symmetric TSP
                }
            }

            return distanceMatrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CalculateDistanceBetween(int indexA, int indexB)
        {
            return _matrix.Value[indexA, indexB];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CalculateTotalDistance(int[] routeIndices)
        {
            double dist = 0;
            for (int i = 1; i < routeIndices.Length; i++)
            {
                dist += _matrix.Value[routeIndices[i - 1], routeIndices[i]];
            }
            return dist;
        }
    }
}
