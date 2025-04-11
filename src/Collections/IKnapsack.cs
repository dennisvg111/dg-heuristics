using System.Collections.Generic;

namespace DG.Heuristic.Collections
{
    /// <summary>
    /// Represents a collection of data that can be used to pick data with a given "weight" closest to some target.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IKnapsack<TData> where TData : IWeightedData
    {
        /// <summary>
        /// <para>Adds the values in the given <paramref name="data"/> to this knapsack.</para>
        /// <para>Note that values with a weight of 0 or less will be ignored.</para>
        /// </summary>
        /// <param name="data"></param>
        void Add(IEnumerable<TData> data);

        /// <summary>
        /// Removes all data currently in this knapsack
        /// </summary>
        void Clear();

        /// <summary>
        /// Finds the sum of values closest to the target. Returns the exact <paramref name="sum"/> of those values.
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        List<TData> PickClosest(out int sum);
    }
}