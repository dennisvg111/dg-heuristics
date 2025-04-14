using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DG.Heuristic.Examples
{
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

        public List<TestResult> RunSingle()
        {
            List<TestResult> results = new List<TestResult>();

            var data = _dataGenerator.GenerateTestData(_random);
            foreach (var runner in _tests)
            {
                _stopwatch.Restart();
                var result = runner.Run(data);
                _stopwatch.Stop();
                results.Add(new TestResult(runner.Name, _stopwatch.Elapsed, _scoreCalculator.CalculateTestScore(data, result)));
            }
            return results;
        }

        public List<TestResult> RunMultiple(int count)
        {
            var resultsList = _tests.Select(t => new TestResult(t.Name)).ToList();
            for (int i = 0; i < count; i++)
            {
                var results = RunSingle();
                for (int runnerI = 0; runnerI < results.Count; runnerI++)
                {
                    resultsList[runnerI].Combine(results[runnerI]);
                }
            }
            return resultsList;
        }
    }

    public static class TestComparator
    {
        public static TestComparator<TInput, TOutput> For<TInput, TOutput>(ITestDataGenerator<TInput> generator, ITestScoreCalculator<TInput, TOutput> calculator)
        {
            return new TestComparator<TInput, TOutput>(generator, calculator);
        }
    }
}
