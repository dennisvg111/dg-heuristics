using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Collections
{
    public class SubsetSumKnapsack
    {
        private readonly int _target;
        private List<int>[] _sums;

        public SubsetSumKnapsack(int target)
        {
            _target = target;
            _sums = new List<int>[2 * target + 1];
            Clear();
        }

        public List<int> PickClosest()
        {
            for (int d = 0; d <= _target; d++)
            {
                if (_sums[_target - d] != null)
                {
                    return _sums[_target - d];
                }
                if (_sums[_target + d] != null)
                {
                    return _sums[_target + d];
                }
            }
            return _sums[0];
        }

        public Dictionary<int, List<int>> GetCalculatedSums()
        {
            return _sums.Select((value, index) => new { Sum = index, Values = value }).Where(s => s.Values != null).ToDictionary(s => s.Sum, s => s.Values);
        }

        public List<int>[] Add(IEnumerable<int> data)
        {
            foreach (int currentValue in data)
            {
                if (currentValue == 0)
                {
                    continue;
                }
                for (int previouslyCalculatedSum = _sums.Length - currentValue - 1; previouslyCalculatedSum >= 0; previouslyCalculatedSum--)
                {
                    var newSum = previouslyCalculatedSum + currentValue;
                    if (SumHasBeenCalculated(newSum) || !SumHasBeenCalculated(previouslyCalculatedSum))
                    {
                        continue;
                    }

                    List<int> valuesForNewSum = new List<int>(_sums[previouslyCalculatedSum]);
                    valuesForNewSum.Add(currentValue);
                    _sums[previouslyCalculatedSum + currentValue] = valuesForNewSum;
                }
            }
            return _sums;
        }

        private bool SumHasBeenCalculated(int sumValue)
        {
            return _sums[sumValue] != null;
        }

        public void Clear()
        {
            Array.Clear(_sums, 0, _sums.Length);
            _sums[0] = new List<int>();
        }
    }
}
