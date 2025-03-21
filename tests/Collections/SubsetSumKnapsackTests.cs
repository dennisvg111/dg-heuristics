using DG.Heuristic.Collections;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DG.Heuristic.Tests.Collections
{
    public class SubsetSumKnapsackTests
    {
        [Fact]
        public void NewSubsetSumKnapsack_TargetNegative_Throws()
        {
            Action create = () => new SubsetSumKnapsack(-4);

            create.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(21)]
        [InlineData(23)]
        public void Add_Positive_ReturnsTrue(int value)
        {
            var knapsack = new SubsetSumKnapsack(21);

            knapsack.Add(value).Should().BeTrue();
        }

        [Fact]
        public void Add_Negative_ReturnsFalse()
        {
            var knapsack = new SubsetSumKnapsack(21);

            knapsack.Add(-3).Should().BeFalse();
        }

        [Fact]
        public void Add_Zero_ReturnsFalse()
        {
            var knapsack = new SubsetSumKnapsack(21);

            knapsack.Add(0).Should().BeFalse();
        }

        [Fact]
        public void PickClosest_TargetZero_ReturnsLowest()
        {
            var knapsack = new SubsetSumKnapsack(0);
            knapsack.Add(new int[] { 2, 3 });

            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(2);
            values.Should().ContainSingle();
            values[0].Should().Be(2);
        }

        [Fact]
        public void PickClosest_Empty_ReturnsZero()
        {
            var knapsack = new SubsetSumKnapsack(21);

            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(0);
            values.Should().BeEmpty();
        }

        [Fact]
        public void PickClosest_ExactPossible_ReturnsCorrect()
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(new int[] { 9, 9, 3 });


            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(21);
            values.Should().HaveCount(3);
            values.OrderBy(v => v).Should().BeEquivalentTo(new int[] { 3, 9, 9 });
        }

        [Fact]
        public void PickClosest_TargetInCollection_ReturnsCorrect()
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(new int[] { 9, 21, 3 });

            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(21);
            values.Should().ContainSingle();
            values[0].Should().Be(21);
        }

        [Fact]
        public void PickClosest_MoreThanNeeded_IgnoresExtra()
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(new int[] { 9, 5, 9, 3 });

            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(21);
            values.Should().NotContain(5);
        }

        [Fact]
        public void PickClosest_LargerPossible_ShouldReturnClosest()
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(new int[] { 9, 5, 9, 4 });

            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(22);
            values.OrderBy(v => v).Should().BeEquivalentTo(new int[] { 4, 9, 9 });
        }

        [Fact]
        public void PickClosest_SmallerPossible_ShouldReturnClosest()
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(new int[] { 9, 2, 9, 5 });

            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(20);
            values.OrderBy(v => v).Should().BeEquivalentTo(new int[] { 2, 9, 9 });
        }

        [Fact]
        public void PickClosest_NumbersAboveNx2Plus1_CanBeFound()
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(3000);

            knapsack.PickClosest(out int sum);

            sum.Should().Be(3000);
        }

        [Theory]
        [InlineData(3000, 9001)]
        [InlineData(9001, 3000)]
        public void PickClosest_NumbersAboveNx2Plus1_ShouldReturnLowest(int largeNumber1, int largeNumber2)
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(largeNumber1);
            knapsack.Add(largeNumber2);

            var values = knapsack.PickClosest(out int sum);

            sum.Should().Be(3000);
            values.Should().ContainSingle();
            values[0].Should().Be(3000);
        }
    }
}
