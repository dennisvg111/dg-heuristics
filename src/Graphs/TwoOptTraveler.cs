using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Graphs
{
    public class TwoOptTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData> where TData : IGraphPoint<TData>
    {
        private readonly List<TData> _points;

        public TwoOptTraveler(IEnumerable<TData> points)
        {
            _points = points.ToList();
        }

        public TwoOptTraveler() : this(new List<TData>())
        {
        }

        public bool Add(TData item)
        {
            _points.Add(item);
            return true;
        }

        public void Clear()
        {
            _points.Clear();
        }

        public List<TData> CalculateRoute(out double totalDistance)
        {
            // Start with the input order
            var route = new List<TData>(_points);
            bool improved = true;

            while (improved)
            {
                improved = false;

                for (int i = 1; i < route.Count - 1; i++)
                {
                    for (int k = i + 1; k < route.Count; k++)
                    {
                        var newRoute = TwoOptSwap(route, i, k);
                        if (RouteDistance(newRoute) < RouteDistance(route))
                        {
                            route = newRoute;
                            improved = true;
                        }
                    }
                }
            }

            totalDistance = RouteDistance(route);
            return route;
        }

        private List<TData> TwoOptSwap(List<TData> route, int i, int k)
        {
            var newRoute = route.Take(i).ToList();
            newRoute.AddRange(route.Skip(i).Take(k - i + 1).Reverse());
            newRoute.AddRange(route.Skip(k + 1));
            return newRoute;
        }

        private double RouteDistance(List<TData> route)
        {
            double dist = 0;
            for (int i = 0; i < route.Count - 1; i++)
                dist += route[i].DistanceTo(route[i + 1]);
            return dist;
        }
    }
}
