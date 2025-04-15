using System;
using System.Collections.Generic;

namespace DG.Heuristic.Examples.Tsp;

public class TspDataGenerator : ITestDataGenerator<List<GraphPoint>>
{
    private static readonly TspDataGenerator _default = new TspDataGenerator(7, 10);
    public static TspDataGenerator Default => _default;

    private readonly int _minPoints;
    private readonly int _maxPoints;

    public int MinPoints => _minPoints;
    public int MaxPoints => _maxPoints;

    /// <summary>
    /// Initializes a new instance of <see cref="TspDataGenerator"/> that generates at least <paramref name="minPoints"/> and at most <paramref name="maxPoints"/> points.
    /// </summary>
    /// <param name="minPoints"></param>
    /// <param name="maxPoints"></param>
    public TspDataGenerator(int minPoints = 7, int maxPoints = 10)
    {
        _minPoints = minPoints;
        _maxPoints = maxPoints;
    }

    public List<GraphPoint> GenerateTestData(Random random)
    {
        List<GraphPoint> points = new List<GraphPoint>();
        int count = random.Next(_minPoints, _maxPoints + 1);
        for (int i = 0; i < count; i++)
        {
            points.Add(new GraphPoint(random.Next(0, 100), random.Next(0, 100)));
        }
        return points;
    }
}
