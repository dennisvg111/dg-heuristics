using DG.Heuristic.Collections;
using System.Collections.Generic;

namespace DG.Heuristic.Examples.Test.Knapsack
{
    public class KnapsackData
    {
        private readonly int _target;
        private readonly List<TestData> _data;

        public int Target => _target;
        public IReadOnlyCollection<TestData> Data => _data;

        public KnapsackData(int target, List<TestData> data)
        {
            _target = target;
            _data = data;
        }

        public class TestData : IWeightedData
        {
            private readonly int _weight;

            public int Weight => _weight;

            public TestData(int weight)
            {
                _weight = weight;
            }
        }
    }
}
