using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Heuristic.Collections
{
    public class SubsetSumKnapsack
    {
        private readonly SubsetSumKnapsack<IntData> _internalKnapsack;

        /// <summary>
        /// <inheritdoc cref="SubsetSumKnapsack{TData}(int)"/>
        /// </summary>
        /// <param name="target"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SubsetSumKnapsack(int target)
        {
            _internalKnapsack = new SubsetSumKnapsack<IntData>(target);
        }

        /// <inheritdoc cref="SubsetSumKnapsack{TData}.Clear()"/>
        public void Clear()
        {
            _internalKnapsack.Clear();
        }

        /// <summary>
        /// <inheritdoc cref="SubsetSumKnapsack{TData}.Add(IEnumerable{TData})"/>
        /// </summary>
        /// <param name="data"></param>
        public void Add(IEnumerable<int> data)
        {
            _internalKnapsack.Add(data.Select(v => new IntData(v)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(int value)
        {
            return _internalKnapsack.Add(value);
        }

        /// <summary>
        /// <inheritdoc cref="SubsetSumKnapsack{TData}.PickClosestTo(int, out int)"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal List<int> PickClosestTo(int target, out int sum)
        {
            return _internalKnapsack.PickClosestTo(target, out sum).Select(d => d.Weight).ToList();
        }

        /// <summary>
        /// <inheritdoc cref="SubsetSumKnapsack{TData}.PickClosest(out int)"/>
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public List<int> PickClosest(out int sum)
        {
            return _internalKnapsack.PickClosest(out sum).Select(d => d.Weight).ToList();
        }

        private class IntData : IWeightedData
        {
            private readonly int _value;
            public int Weight => _value;

            public IntData(int value)
            {
                _value = value;
            }

            public static implicit operator IntData(int value) => new IntData(value);
            public static implicit operator int(IntData value) => value.Weight;
        }
    }

    public class SubsetSumKnapsack<TData> where TData : IWeightedData
    {
        private readonly int _target;
        private int _lastHighest;
        private List<TData>[] _sums;
        private TData _closest;

        /// <summary>
        /// Creates a new knapsack with the given target.
        /// </summary>
        /// <param name="target"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SubsetSumKnapsack(int target)
        {
            if (target < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(target), $"Value should not be negative.");
            }
            _target = target;
            _sums = new List<TData>[CalculateMaxCapacity(target)];
            Clear();
        }

        /// <summary>
        /// Removes all data currently in this knapsack
        /// </summary>
        public void Clear()
        {
            Array.Clear(_sums, 0, _sums.Length);
            _lastHighest = 0;
            _sums[0] = new List<TData>();
            _closest = default;
        }

        /// <summary>
        /// <para>Adds the values in the given <paramref name="data"/> to this knapsack.</para>
        /// <para>Note that values of 0 or less will be ignored.</para>
        /// </summary>
        /// <param name="data"></param>
        public void Add(IEnumerable<TData> data)
        {
            foreach (var currentValue in data)
            {
                Add(currentValue);
            }
        }

        /// <summary>
        /// <para>Adds the given <paramref name="value"/> to this knapsack.</para>
        /// <para>Note that a value of 0 or less will be ignored.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns a value indicating indicating if this <paramref name="value"/> was added (<see langword="true"/>), or ignored (<see langword="false"/>).</returns>
        public bool Add(TData value)
        {
            if (value.Weight <= 0)
            {
                return false;
            }

            SaveClosestIfRelevant(value);
            var start = Math.Min(_lastHighest, _sums.Length - value.Weight - 1);
            _lastHighest = start + value.Weight;
            for (int previouslyCalculatedSum = start; previouslyCalculatedSum >= 0; previouslyCalculatedSum--)
            {
                if (!IsSumSaved(previouslyCalculatedSum))
                {
                    continue;
                }
                var newSum = previouslyCalculatedSum + value.Weight;
                if (IsSumSaved(newSum))
                {
                    continue;
                }

                _sums[newSum] = CopySumAndAddValue(previouslyCalculatedSum, value);
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
        internal List<TData> PickClosestTo(int target, out int sum)
        {
            if (target > _sums.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(target), $"Value should be less than {_sums.Length}.");
            }
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

            if (_closest != null)
            {
                sum = _closest.Weight;
                return new List<TData> { _closest };
            }

            sum = 0;
            return _sums[0];
        }

        /// <summary>
        /// Finds the sum of values closest to the target. Returns the exact <paramref name="sum"/> of those values.
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        public List<TData> PickClosest(out int sum)
        {
            return PickClosestTo(_target, out sum);
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

        private void SaveClosestIfRelevant(TData value)
        {
            if (_closest == null)
            {
                _closest = value;
                return;
            }
            if (Math.Abs(_target - value.Weight) < Math.Abs(_target - _closest.Weight))
            {
                _closest = value;
            }
        }

        private static int CalculateMaxCapacity(int target)
        {
            return 2 * target + 1;
        }
    }
}
