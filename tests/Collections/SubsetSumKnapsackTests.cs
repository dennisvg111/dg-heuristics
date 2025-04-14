using DG.Heuristic.Collections;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static DG.Heuristic.Tests.Collections.SubsetSumKnapsackTests;

namespace DG.Heuristic.Tests.Collections
{
    public class SubsetSumKnapsackTests
    {
        [Fact]
        public void PickClosestTo_TargetNegative_Throws()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();

            Action action = () => knapsack.PickClosestTo(-4, out int sum);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(21)]
        [InlineData(23)]
        public void Add_Positive_ReturnsTrue(int value)
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();

            knapsack.Add(value).Should().BeTrue();
        }

        [Fact]
        public void Add_Negative_ReturnsFalse()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();

            knapsack.Add(-3).Should().BeFalse();
        }

        [Fact]
        public void Add_Zero_ReturnsFalse()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();

            knapsack.Add(0).Should().BeFalse();
        }

        [Fact]
        public void PickClosestTo_TargetZero_ReturnsLowest()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(new int[] { 2, 3 });

            var values = knapsack.PickClosestTo(0, out int sum);

            sum.Should().Be(2);
            values.Should().ContainSingle();
            TestKnapsackExtensions.Be(values[0].Should<TestIntData>(), 2);
        }

        [Fact]
        public void PickClosestTo_Empty_ReturnsZero()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();

            var values = knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(0);
            values.Should().BeEmpty();
        }

        [Fact]
        public void PickClosestTo_ExactPossible_ReturnsCorrect()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(new int[] { 9, 9, 3 });


            var values = knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(21);
            values.Should().HaveCount(3)
                .And.BeEquivalentTo(3, 9, 9);
        }

        [Fact]
        public void PickClosestTo_TargetInCollection_ReturnsCorrect()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(new int[] { 9, 21, 3 });

            var values = knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(21);
            values.Should().ContainSingle();
            values[0].Should().Be(21);
        }

        [Fact]
        public void PickClosestTo_MoreThanNeeded_IgnoresExtra()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(new int[] { 9, 5, 9, 3 });

            var values = knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(21);
            values.Should().NotContain(5);
        }

        [Fact]
        public void PickClosestTo_LargerPossible_ShouldReturnClosest()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(new int[] { 9, 5, 9, 4 });

            var values = knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(22);
            values.Should().BeEquivalentTo(4, 9, 9);
        }

        [Fact]
        public void PickClosestTo_SmallerPossible_ShouldReturnClosest()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(new int[] { 9, 2, 9, 5 });

            var values = knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(20);
            values.Should().BeEquivalentTo(2, 9, 9);
        }

        [Fact]
        public void PickClosestTo_NumbersAboveNx2Plus1_CanBeFound()
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(3000);

            knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(3000);
        }

        [Theory]
        [InlineData(3000, 9001)]
        [InlineData(9001, 3000)]
        public void PickClosestTo_NumbersAboveNx2Plus1_ShouldReturnLowest(int largeNumber1, int largeNumber2)
        {
            var knapsack = new SubsetSumKnapsack<TestIntData>();
            knapsack.Add(largeNumber1);
            knapsack.Add(largeNumber2);

            var values = knapsack.PickClosestTo(21, out int sum);

            sum.Should().Be(3000);
            values.Should().ContainSingle();
            values[0].Should().Be(3000);
        }



        internal class TestIntData : IWeightedData, IComparable<TestIntData>, IComparable, IEquatable<TestIntData>
        {
            private readonly int _value;
            public int Weight => _value;

            public TestIntData(int value)
            {
                _value = value;
            }

            public override string ToString()
            {
                return "" + _value;
            }

            public int CompareTo(TestIntData other)
            {
                return _value.CompareTo(other._value);
            }

            public int CompareTo(object obj)
            {
                if (obj is TestIntData iData)
                {
                    return CompareTo(iData);
                }
                if (obj is int i)
                {
                    return _value.CompareTo(i);
                }
                return 1;
            }

            public bool Equals(TestIntData other)
            {
                return _value.Equals(other._value);
            }

            public override bool Equals(object obj)
            {
                if (obj is TestIntData iData)
                {
                    return Equals(iData);
                }
                if (obj is int i)
                {
                    return _value.Equals(i);
                }
                return false;
            }
        }
    }

    internal static class TestKnapsackExtensions
    {
        internal static void Add(this SubsetSumKnapsack<TestIntData> knapsack, IEnumerable<int> data)
        {
            foreach (var i in data)
            {
                knapsack.Add(i);
            }
        }
        internal static bool Add(this SubsetSumKnapsack<TestIntData> knapsack, int data)
        {
            return knapsack.Add(new TestIntData(data));
        }

        internal static TestIntData[] ToTestData(this int[] data)
        {
            return data.Select(i => new TestIntData(i)).ToArray();
        }

        public static AndConstraint<GenericCollectionAssertions<TestIntData>> BeEquivalentTo(this GenericCollectionAssertions<TestIntData> results, params int[] data)
        {
            var expected = data.OrderBy(i => i).Select(i => new TestIntData(i)).ToArray();
            return results.Subject.OrderBy(i => i).Should().BeEquivalentTo(expected);
        }

        public static AndConstraint<ComparableTypeAssertions<TestIntData>> Be(this ComparableTypeAssertions<TestIntData, ComparableTypeAssertions<TestIntData>> result, int data)
        {
            var expected = new TestIntData(data);
            return result.Be(expected);
        }

        public static AndConstraint<GenericCollectionAssertions<TestIntData>> NotContain(this GenericCollectionAssertions<TestIntData> results, int data)
        {

            var expected = new TestIntData(data);
            return results.NotContainEquivalentOf(expected);
        }
    }
}