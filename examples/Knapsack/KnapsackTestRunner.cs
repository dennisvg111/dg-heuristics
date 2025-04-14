using DG.Heuristic.Collections;
using System;

namespace DG.Heuristic.Examples.Knapsack
{
    public class KnapsackTestRunner<TKnapsack> : ITestRunner<KnapsackData, int> where TKnapsack : IKnapsack<KnapsackData.TestData>
    {
        private readonly Func<KnapsackData, TKnapsack> _knapsackFactory;

        public string Name => typeof(TKnapsack).Name.Replace("`1", "");

        public KnapsackTestRunner(Func<KnapsackData, TKnapsack> knapsackFactory)
        {
            _knapsackFactory = knapsackFactory;
        }

        /// <summary>
        /// Runs the current algorithm for the given <paramref name="data"/>, and returns a score indicating how good this result is (higher is better).
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A score indicating how good this result is (higher is better).</returns>
        public int Run(KnapsackData data)
        {
            var knapsack = _knapsackFactory(data);

            var unusedResults = knapsack.PickClosestTo(data.Target, out int foundSum);
            return foundSum;
        }
    }

    public static class KnapsackTestRunner
    {
        public static KnapsackTestRunner<TKnapsack> For<TKnapsack>() where TKnapsack : IKnapsack<KnapsackData.TestData>, IMutableCollection<KnapsackData.TestData>, new()
        {
            return For((data) =>
            {
                var knapsack = new TKnapsack();
                knapsack.AddRange(data.Data);
                return knapsack;
            });
        }

        public static KnapsackTestRunner<TKnapsack> For<TKnapsack>(Func<KnapsackData, TKnapsack> knapsackFactory) where TKnapsack : IKnapsack<KnapsackData.TestData>
        {
            return new KnapsackTestRunner<TKnapsack>(knapsackFactory);
        }
    }
}
