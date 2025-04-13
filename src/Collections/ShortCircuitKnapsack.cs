using System.Collections.Generic;

namespace DG.Heuristic.Collections
{
    public class ShortCircuitKnapsack<TData> : IKnapsack<TData> where TData : IWeightedData
    {
        private int _target;
        private SortedList<TData, TData> _sortedData = new SortedList<TData, TData>(new InternalComparer());

        public ShortCircuitKnapsack(int target)
        {
            _target = target;
        }

        public void Add(IEnumerable<TData> data)
        {
            foreach (var item in data)
            {
                _sortedData.Add(item, item);
            }
        }

        public void Clear()
        {
            _sortedData.Clear();
        }

        public List<TData> PickClosest(out int sum)
        {
            List<TData> data = new List<TData>();
            sum = 0;

            foreach (var item in _sortedData)
            {
                sum += item.Value.Weight;
                data.Add(item.Value);
                if (sum >= _target)
                {
                    break;
                }
            }

            return data;
        }

        public class InternalComparer : IComparer<TData>
        {
            public int Compare(TData x, TData y)
            {
                if (x.Weight == y.Weight)
                {
                    return 1;
                }
                return x.Weight.CompareTo(y.Weight);
            }
        }
    }
}
