namespace DG.Heuristic.Examples
{
    public interface ITestScoreCalculator<TData, TResult>
    {
        double CalculateTestScore(TData data, TResult testResult);
    }
}
