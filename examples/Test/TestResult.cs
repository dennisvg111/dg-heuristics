using System;

namespace DG.Heuristic.Examples.Test
{
    public class TestResult
    {
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Lower should be better.
        /// </summary>
        public float Score { get; set; }
    }
}
