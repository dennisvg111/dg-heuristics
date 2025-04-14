using DG.Heuristic.Collections;
using System;

namespace DG.Heuristic.Examples.Knapsack
{
    public class KnapsackTestRunner<TKnapsack> : ITestRunner<KnapsackData, int> where TKnapsack : IKnapsack<KnapsackData.TestData>
    {
        private readonly Func<int, TKnapsack> _knapsackFactory;

        public KnapsackTestRunner(Func<int, TKnapsack> knapsackFactory)
        {
            _knapsackFactory = knapsackFactory;
        }

        public string Name => typeof(TKnapsack).Name.Replace("`1", "");
        /// <summary>
        /// Runs the current algorithm for the given <paramref name="data"/>, and returns a score indicating how good this result is (higher is better).
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A score indicating how good this result is (higher is better).</returns>
        public int Run(KnapsackData data)
        {
            var knapsack = _knapsackFactory(data.Target);
            knapsack.Add(data.Data);

            var unusedResults = knapsack.PickClosest(out int foundSum);
            return foundSum;
        }
    }

    public static class KnapsackTestRunner
    {
        public static KnapsackTestRunner<TKnapsack> For<TKnapsack>(Func<int, TKnapsack> knapsackFactory) where TKnapsack : IKnapsack<KnapsackData.TestData>
        {
            return new KnapsackTestRunner<TKnapsack>(knapsackFactory);
        }
    }
}
