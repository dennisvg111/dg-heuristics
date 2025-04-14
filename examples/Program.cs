using DG.Heuristic.Collections;
using DG.Heuristic.Examples.Knapsack;
using System;

namespace DG.Heuristic.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var comparator = TestComparator.For(new KnapsackDataGenerator(), new KnapsackScoreCalculator());

            comparator.AddTest(KnapsackTestRunner.For<ShortCircuitKnapsack<KnapsackData.TestData>>());
            comparator.AddTest(KnapsackTestRunner.For<SubsetSumKnapsack<KnapsackData.TestData>>());

            int testCount = 1000;
            Console.WriteLine($"Running {testCount} tests");
            var results = comparator.RunMultiple(testCount);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
