using System;
using System.Collections.Generic;

namespace DG.Heuristic.Collections
{
    public class SubsetSumKnapsack
    {
        private readonly int _target;
        private int _lastHighest;
        private List<int>[] _sums;

        public SubsetSumKnapsack(int target)
        {
            _target = target;
            _sums = new List<int>[CalculateMaxCapacity(target)];
            Clear();
        }

        public void Clear()
        {
            Array.Clear(_sums, 0, _sums.Length);
            _lastHighest = 0;
            _sums[0] = new List<int>();
        }

        public void Add(IEnumerable<int> data)
        {
            foreach (int currentValue in data)
            {
                Add(currentValue);
            }
        }

        public void Add(int value)
        {
            if (value == 0)
            {
                return;
            }
            var start = Math.Min(_lastHighest, _sums.Length - value - 1);
            _lastHighest = start + value;
            for (int previouslyCalculatedSum = start; previouslyCalculatedSum >= 0; previouslyCalculatedSum--)
            {
                if (!IsSumSaved(previouslyCalculatedSum))
                {
                    continue;
                }
                var newSum = previouslyCalculatedSum + value;
                if (IsSumSaved(newSum))
                {
                    continue;
                }

                _sums[newSum] = CopySumAndAddValue(previouslyCalculatedSum, value);
            }
        }

        public List<int> PickClosestTo(int target, out int sum)
        {
            if (target > _sums.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(target), $"Value should be less than {_sums.Length}.");
            }
            for (int d = 0; d <= target; d++)
            {
                if (IsSumSaved(target - d))
                {
                    sum = target - d;
                    return _sums[sum];
                }
                if (IsSumSaved(target + d))
                {
                    sum = target + d;
                    return _sums[sum];
                }
            }

            sum = 0;
            return _sums[0];
        }

        public List<int> PickClosest(out int sum)
        {
            return PickClosestTo(_target, out sum);
        }

        private bool IsSumSaved(int sumValue)
        {
            return _sums[sumValue] != null;
        }

        private List<int> CopySumAndAddValue(int previousSum, int newValue)
        {
            return new List<int>(_sums[previousSum])
            {
                newValue
            };
        }

        private static int CalculateMaxCapacity(int target)
        {
            return 2 * target + 1;
        }
    }
}
