using iluvadev.ConsoleProgressBar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DG.Heuristic.Examples;

public class TestComparator<TInput, TOutput>
{
    private static readonly Random _random = new Random();
    private readonly List<ITestRunner<TInput, TOutput>> _tests = new List<ITestRunner<TInput, TOutput>>();
    private readonly Stopwatch _stopwatch = new Stopwatch();

    private readonly ITestDataGenerator<TInput> _dataGenerator;
    private readonly ITestScoreCalculator<TInput, TOutput> _scoreCalculator;

    public TestComparator(ITestDataGenerator<TInput> dataGenerator, ITestScoreCalculator<TInput, TOutput> scoreCalculator)
    {
        _dataGenerator = dataGenerator;
        _scoreCalculator = scoreCalculator;
    }

    public void AddTest(ITestRunner<TInput, TOutput> test)
    {
        _tests.Add(test);
    }

    private List<TestResult> RunSingle(ProgressBar pb)
    {
        List<TestResult> results = new List<TestResult>();

        var data = _dataGenerator.GenerateTestData(_random);
        foreach (var runner in _tests)
        {
            string runnerName = runner.Name;
            pb.ElementName = runner.Name;
            _stopwatch.Restart();
            var result = runner.Run(data);
            _stopwatch.Stop();
            pb.PerformStep();
            results.Add(new TestResult(runnerName, _stopwatch.Elapsed, _scoreCalculator.CalculateTestScore(data, result)));
        }
        return results;
    }

    public List<TestResult> RunMultiple(int count)
    {
        var resultsList = _tests.Select(t => new TestResult(t.Name)).ToList();
        using (var pb = CreateProgressBar(count))
        {
            for (int i = 0; i < count; i++)
            {
                var results = RunSingle(pb);
                for (int runnerI = 0; runnerI < results.Count; runnerI++)
                {
                    resultsList[runnerI].Combine(results[runnerI]);
                }
            }
        }
        return resultsList;
    }

    private ProgressBar CreateProgressBar(int runCount)
    {
        var pb = new ProgressBar()
        {
            Maximum = runCount * _tests.Count
        };
        pb.Layout.Body.Pending.SetValue('─');
        pb.Layout.Marquee.OverPending.SetValue('─');
        pb.Text.Description.Clear();
        pb.Text.Description.Processing.AddNew().SetValue(pb => $"Testing: {pb.ElementName}");
        pb.Text.Description.Done.AddNew().SetValue(pb => $"{pb.Value} tests in {pb.TimeProcessing.TotalSeconds}s.");
        Thread.Sleep(100);
        return pb;
    }
}

public static class TestComparator
{
    public static TestComparator<TInput, TOutput> For<TInput, TOutput>(ITestDataGenerator<TInput> generator, ITestScoreCalculator<TInput, TOutput> calculator)
    {
        return new TestComparator<TInput, TOutput>(generator, calculator);
    }
}
