using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Graphs
{
    public class BruteForceTraveler<TData> : IGraphTraveler<TData>, IMutableCollection<TData> where TData : IGraphPoint<TData>
    {
        private readonly List<TData> _points;

        public BruteForceTraveler() : this(new List<TData>())
        {
        }

        public BruteForceTraveler(IEnumerable<TData> points)
        {
            _points = points.ToList();
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
            totalDistance = double.MaxValue;
            List<TData> bestRoute = null;

            foreach (var perm in GetPermutations(_points))
            {
                double dist = 0;
                for (int i = 0; i < perm.Count - 1; i++)
                    dist += perm[i].DistanceTo(perm[i + 1]);

                if (dist < totalDistance)
                {
                    totalDistance = dist;
                    bestRoute = perm;
                }
            }

            return bestRoute ?? new List<TData>();
        }

        private IEnumerable<List<TData>> GetPermutations(List<TData> list)
        {
            if (list.Count == 1)
                yield return new List<TData>(list);
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var elem = list[i];
                    var rest = new List<TData>(list);
                    rest.RemoveAt(i);

                    foreach (var perm in GetPermutations(rest))
                    {
                        perm.Insert(0, elem);
                        yield return perm;
                    }
                }
            }
        }
    }
}
