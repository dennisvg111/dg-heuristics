using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Collections
{
    public class SubsetSumKnapsack<TData> : IKnapsack<TData>, IMutableCollection<TData> where TData : IWeightedData
    {
        private List<TData> _items;
        private int _lastHighest;
        private List<TData>[] _sums;

        /// <summary>
        /// Creates a new knapsack.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SubsetSumKnapsack()
        {
            Clear();
        }

        private void ResetSums(int capacity)
        {
            _lastHighest = 0;
            _sums = new List<TData>[capacity];
            _sums[0] = new List<TData>();
        }

        /// <summary>
        /// Removes all data currently in this knapsack
        /// </summary>
        public void Clear()
        {
            _items = new List<TData>();
            ResetSums(1);
        }

        /// <summary>
        /// <inheritdoc cref="IMutableCollection{T}.Add(T)"/>
        /// <para>Note that items with a weight of 0 or less will always be ignored.</para>
        /// </summary>
        /// <param name="item"><inheritdoc cref="IMutableCollection{T}.Add(T)"/></param>
        /// <returns><inheritdoc cref="IMutableCollection{T}.Add(T)"/></returns>
        public bool Add(TData item)
        {
            if (item.Weight <= 0)
            {
                return false;
            }
            _items.Add(item);
            return true;
        }

        private void CalculateSumsIfNeeded(int target)
        {
            var capacity = CalculateMaxCapacity(target);
            if (_sums != null && capacity <= _sums.Length)
            {
                return;
            }

            ResetSums(capacity);

            foreach (var item in _items)
            {
                CalculateNewSums(item);
            }
        }

        private bool CalculateNewSums(TData item)
        {
            if (item.Weight <= 0)
            {
                return false;
            }

            var start = Math.Min(_lastHighest, _sums.Length - item.Weight - 1);
            _lastHighest = start + item.Weight;
            for (int previouslyCalculatedSum = start; previouslyCalculatedSum >= 0; previouslyCalculatedSum--)
            {
                if (!IsSumSaved(previouslyCalculatedSum))
                {
                    continue;
                }
                var newSum = previouslyCalculatedSum + item.Weight;
                if (IsSumSaved(newSum))
                {
                    continue;
                }

                _sums[newSum] = CopySumAndAddValue(previouslyCalculatedSum, item);
            }
            return true;
        }

        /// <summary>
        /// <para>Finds the sum closest to a given target. Returns the exact <paramref name="sum"/> of those values.</para>
        /// <para>Note that this throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="target"/> is more than <c>n*2+1</c>, where <c>n</c> is the original target of this knapsack.</para>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public List<TData> PickClosestTo(int target, out int sum)
        {
            if (target < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(target), $"Value should not be negative.");
            }

            CalculateSumsIfNeeded(target);

            for (int d = 0; d <= target; d++)
            {
                if (IsSumSaved(target - d) && target - d != 0)
                {
                    sum = target - d;
                    return _sums[sum];
                }
                if (d > 0 && IsSumSaved(target + d))
                {
                    sum = target + d;
                    return _sums[sum];
                }
            }

            if (_items.Any())
            {
                var lowest = _items.OrderBy(i => i.Weight).Take(1).ToList();
                sum = lowest[0].Weight;
                return lowest;
            }

            sum = 0;
            return _sums[0];
        }

        private bool IsSumSaved(int sumValue)
        {
            return _sums[sumValue] != null;
        }

        private List<TData> CopySumAndAddValue(int previousSum, TData newValue)
        {
            return new List<TData>(_sums[previousSum])
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
