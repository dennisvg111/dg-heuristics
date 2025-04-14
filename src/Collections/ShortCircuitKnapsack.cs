using System.Collections.Generic;

namespace DG.Heuristic.Collections
{
    public class ShortCircuitKnapsack<TData> : IKnapsack<TData>, IMutableCollection<TData> where TData : IWeightedData
    {
        private SortedList<TData, TData> _sortedData = new SortedList<TData, TData>(new InternalComparer());

        /// <inheritdoc/>
        public bool Add(TData item)
        {
            _sortedData.Add(item, item);
            return true;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _sortedData.Clear();
        }

        /// <inheritdoc/>
        public List<TData> PickClosestTo(int target, out int sum)
        {
            List<TData> data = new List<TData>();
            sum = 0;

            foreach (var item in _sortedData)
            {
                sum += item.Value.Weight;
                data.Add(item.Value);
                if (sum >= target)
                {
                    break;
                }
            }

            return data;
        }

        private class InternalComparer : IComparer<TData>
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
