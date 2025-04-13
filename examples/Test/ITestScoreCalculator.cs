namespace DG.Heuristic.Examples.Test
{
    public interface ITestScoreCalculator<TData, TResult>
    {
        double CalculateTestScore(TData data, TResult testResult);
    }
}
