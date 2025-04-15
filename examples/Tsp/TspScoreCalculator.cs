using System.Collections.Generic;

namespace DG.Heuristic.Examples.Tsp
{
    public class TspScoreCalculator : ITestScoreCalculator<List<GraphPoint>, double>
    {
        private static readonly TspScoreCalculator _instance = new TspScoreCalculator();

        public static TspScoreCalculator Instance => _instance;

        public double CalculateTestScore(List<GraphPoint> data, double testResult)
        {
            return testResult;
        }
    }
}
