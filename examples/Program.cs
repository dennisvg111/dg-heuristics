using DG.Heuristic.Collections;
using DG.Heuristic.Examples.Knapsack;
using DG.Heuristic.Examples.Tsp;
using DG.Heuristic.Graphs;
using System;
using System.Collections.Generic;

namespace DG.Heuristic.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pointA = new GraphPoint(0, 0);
            var pointB = new GraphPoint(3, 4);
            var distance = pointA.DistanceTo(pointB);

            RunTravelerTests(new WikipediaExampleTspDataGenerator());
            Console.ReadLine();
            RunKnapsackTests();
        }

        private static void RunTravelerTests(ITestDataGenerator<List<GraphPoint>> generator)
        {
            var comparator = TestComparator.For(generator, TspScoreCalculator.Instance);

            comparator.AddTest(TravelerTestRunner.For<BruteForceTraveler<GraphPoint>>());
            comparator.AddTest(TravelerTestRunner.For<NearestNeighbourTraveler<GraphPoint>>());
            comparator.AddTest(TravelerTestRunner.For<TwoOptTraveler<GraphPoint>>());

            int testCount = 100;
            Console.WriteLine($"Running {testCount} tests");
            var results = comparator.RunMultiple(testCount);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        private static void RunKnapsackTests()
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
