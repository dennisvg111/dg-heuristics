using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Graphs
{
    public class NearestNeighbourTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData> where TData : IGraphPoint<TData>
    {
        private readonly List<TData> _points;

        public NearestNeighbourTraveler(IEnumerable<TData> points)
        {
            _points = points.ToList();
        }

        public NearestNeighbourTraveler() : this(new List<TData>())
        {
        }

        /// <inheritdoc />
        public bool Add(TData item)
        {
            _points.Add(item);
            return true;
        }

        public void Clear()
        {
            _points.Clear();
        }

        /// <inheritdoc />
        public List<TData> CalculateRoute(out double totalDistance)
        {
            var route = new List<TData>();
            totalDistance = 0;

            if (!_points.Any())
            {
                return route;
            }

            var unvisited = new HashSet<TData>(_points);
            var current = _points[0];
            route.Add(current);
            unvisited.Remove(current);

            while (unvisited.Count > 0)
            {
                var nearest = unvisited
                    .OrderBy(p => current.DistanceTo(p))
                    .First();

                totalDistance += current.DistanceTo(nearest);
                current = nearest;
                route.Add(current);
                unvisited.Remove(current);
            }

            return route;
        }
    }
}
