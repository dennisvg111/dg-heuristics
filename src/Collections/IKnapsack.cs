using System.Collections.Generic;

namespace DG.Heuristic.Collections
{
    public interface IKnapsack<TData> where TData : IWeightedData
    {
        void Add(IEnumerable<TData> data);
        void Clear();
        List<TData> PickClosest(out int sum);
    }
}