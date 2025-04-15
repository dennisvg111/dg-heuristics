using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Examples.Tsp
{
    public class StaticExampleTspDataGenerator : ITestDataGenerator<List<GraphPoint>>
    {
        private static readonly GraphPoint[] _points = new GraphPoint[]
        {
            new GraphPoint(0, 60),
            new GraphPoint(12, 32),
            new GraphPoint(18, 67),
            new GraphPoint(35, 25),
            new GraphPoint(47, 17),
            new GraphPoint(58, 53),
            new GraphPoint(95, 80)
        };

        public List<GraphPoint> GenerateTestData(Random random)
        {
            return _points
                .OrderBy(p => (random.NextDouble() * 2) - 1)
                .ToList();
        }
    }
}
