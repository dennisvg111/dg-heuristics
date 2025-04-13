using System;

namespace DG.Heuristic.Examples.Test.Knapsack
{
    public class KnapsackScoreCalculator : ITestScoreCalculator<KnapsackData, int>
    {
        public double CalculateTestScore(KnapsackData data, int testResult)
        {
            if (testResult == data.Target)
            {
                return 2;
            }
            return 1d / Math.Abs(data.Target - testResult);
        }
    }
}
