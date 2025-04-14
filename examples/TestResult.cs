using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Examples
{
    public class TestResult
    {
        private readonly List<TimeSpan> _durations;
        private readonly List<double> _scores;

        public string TestName { get; set; }

        public TimeSpan Duration
        {
            get
            {
                var average = _durations.Average(t => t.Ticks);
                long longAverageTicks = Convert.ToInt64(average);

                return new TimeSpan(longAverageTicks);
            }
        }

        /// <summary>
        /// Higher should be better.
        /// </summary>
        public double RelativeScore => _scores.Average();

        public TestResult(string testName)
        {
            TestName = testName;
            _durations = new List<TimeSpan>();
            _scores = new List<double>();
        }

        public TestResult(string testName, TimeSpan duration, double score)
        {
            TestName = testName;
            _durations = new List<TimeSpan>() { duration };
            _scores = new List<double>() { score };
        }

        public void Combine(TestResult result)
        {
            _durations.AddRange(result._durations);
            _scores.AddRange(result._scores);
        }

        public override string ToString()
        {
            return $"[{Duration.ToString()}] {TestName}, Score: {RelativeScore}";
        }
    }
}
