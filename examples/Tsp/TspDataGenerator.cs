using System;
using System.Collections.Generic;

namespace DG.Heuristic.Examples.Tsp;

public class TspDataGenerator : ITestDataGenerator<List<GraphPoint>>
{
    private static readonly TspDataGenerator _default = new TspDataGenerator(10);
    public static TspDataGenerator Default => _default;

    private readonly int _pointCount;

    public int Count => _pointCount;

    /// <summary>
    /// Initializes a new instance of <see cref="TspDataGenerator"/> that generates <paramref name="count"/> points.
    /// </summary>
    /// <param name="count"></param>
    public TspDataGenerator(int count = 7)
    {
        _pointCount = count;
    }

    public List<GraphPoint> GenerateTestData(Random random)
    {
        List<GraphPoint> points = new List<GraphPoint>();
        for (int i = 0; i < _pointCount; i++)
        {
            points.Add(new GraphPoint(random.Next(0, 100), random.Next(0, 100)));
        }
        return points;
    }
}
