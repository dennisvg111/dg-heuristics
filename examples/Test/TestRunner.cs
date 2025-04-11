using DG.Heuristic.Collections;
using DG.Heuristic.Examples.Test.Knapsack;

namespace DG.Heuristic.Examples.Test
{
    public class TestRunner
    {
        public void Run(KnapsackData data)
        {
            var knapsack = new SubsetSumKnapsack<KnapsackData.TestData>(data.Target);
            knapsack.Add(data.Data);

        }
    }
}
