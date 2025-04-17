using DG.Heuristic.Collections;
using DG.Heuristic.Examples.Knapsack;
using DG.Heuristic.Examples.Tsp;
using DG.Heuristic.Graphs.Tsp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DG.Heuristic.Examples;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting tests . . .");
        Console.WriteLine("");
        Thread.Sleep(1000);

        var pointA = new GraphPoint(0, 0);
        var pointB = new GraphPoint(3, 4);
        var distance = pointA.DistanceTo(pointB);

        RunTravelerTests(1000, $"static example points ({StaticExampleTspDataGenerator.PointCount})", new StaticExampleTspDataGenerator());
        var randomTraveledPointsGenerator = new TspDataGenerator(100);
        RunTravelerTests(20, $"randomly generated points ({randomTraveledPointsGenerator.Count})", randomTraveledPointsGenerator);
        RunKnapsackTests();
    }

    private static void RunTravelerTests(int testCount, string dataName, ITestDataGenerator<List<GraphPoint>> generator)
    {
        Console.WriteLine($"Testing TSP algorithm using {dataName}.");
        var comparator = TestComparator.For(generator, TspScoreCalculator.Instance);

        comparator.AddTest(TravelerTestRunner.For<NearestNeighbourTraveler<GraphPoint>>());
        comparator.AddTest(TravelerTestRunner.For<TwoOptTraveler<GraphPoint>>());

        comparator.AddTest(TravelerTestRunner.For<AntColonyTraveler<GraphPoint>>());

        var results = comparator.RunMultiple(testCount);
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
        Console.WriteLine();
    }

    private static void RunKnapsackTests()
    {
        Console.WriteLine($"Testing SubsetSum knapsack algorithm.");
        var comparator = TestComparator.For(new KnapsackDataGenerator(), new KnapsackScoreCalculator());

        comparator.AddTest(KnapsackTestRunner.For<ShortCircuitKnapsack<KnapsackData.TestData>>());
        comparator.AddTest(KnapsackTestRunner.For<SubsetSumKnapsack<KnapsackData.TestData>>());

        int testCount = 1000;
        Console.WriteLine($"{testCount} runs per algorithm");
        var results = comparator.RunMultiple(testCount);
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }
    }
}
