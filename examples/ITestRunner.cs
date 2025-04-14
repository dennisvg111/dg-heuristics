namespace DG.Heuristic.Examples
{
    public interface ITestRunner<TInput, TOutput>
    {
        /// <summary>
        /// The name of the tested algorithm.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Runs the current algorithm for the given <paramref name="input"/>, and returns an output that can be used to calculate a score.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Output of this test run.</returns>
        TOutput Run(TInput input);
    }
}
