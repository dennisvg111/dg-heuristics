using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DG.Heuristic.Graphs
{
    /// <summary>
    /// <para>A utility class to cache the result of distance calculations of collection of <see cref="IGraphPoint{TData}"/>.</para>
    /// <para>This class assumes distances are always symmetrical, e.g. the distance from <c>A</c> to <c>B</c> is the same as the distance from <c>B</c> to <c>A</c>.</para>
    /// </summary>
    /// <typeparam name="TPoint"></typeparam>
    public class CachedDistanceMatrix<TPoint> where TPoint : IGraphPoint<TPoint>
    {
        private double[,] _matrix;
        private IReadOnlyList<TPoint> _points;
        private bool _isCached;

        public CachedDistanceMatrix() : this(Array.Empty<TPoint>())
        {
        }

        public CachedDistanceMatrix(IReadOnlyList<TPoint> points)
        {
            SetPoints(points);
        }

        public void SetPoints(IReadOnlyList<TPoint> data)
        {
            _isCached = false;
            _points = data;
            _matrix = null;
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
                    distanceMatrix[j, i] = distance; // symmetric distances
                }
            }

            _isCached = true;
            return distanceMatrix;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CalculateDistanceBetween(int indexA, int indexB)
        {
            if (!_isCached)
            {
                _matrix = BuildDistanceMatrixIfNeeded();
            }
            return _matrix[indexA, indexB];
        }

        /// <summary>
        /// <para>Calculates the total distance between each of the points in a route, from start to end.</para>
        /// <para>This method assumes a non-cyclic route.</para>
        /// </summary>
        /// <param name="routeIndices"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double CalculateTotalDistance(int[] routeIndices)
        {
            if (!_isCached)
            {
                _matrix = BuildDistanceMatrixIfNeeded();
            }
            double dist = 0;
            for (int i = 1; i < routeIndices.Length; i++)
            {
                dist += _matrix[routeIndices[i - 1], routeIndices[i]];
            }
            return dist;
        }
    }
}
