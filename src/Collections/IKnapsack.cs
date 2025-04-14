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
        /// Finds items that have a sum of weights closest to the <paramref name="target"/>. Returns the exact <paramref name="sum"/> of those values.
        /// </summary>
        /// <param name="target">The target total weight of items to search for.</param>
        /// <param name="sum">The sum of weights of the found items.</param>
        /// <returns></returns>
        List<TData> PickClosestTo(int target, out int sum);
    }
}