using System.Collections.Generic;

namespace DG.Heuristic.Examples.Tsp
{
    public class TspTotalDistanceScoreCalculator : ITestScoreCalculator<List<GraphPoint>, double>
    {
        private static readonly TspTotalDistanceScoreCalculator _instance = new TspTotalDistanceScoreCalculator();

        public static TspTotalDistanceScoreCalculator Instance => _instance;

        public double CalculateTestScore(List<GraphPoint> data, double testResult)
        {
            return testResult;
        }
    }
}
