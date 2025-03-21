using DG.Heuristic.Collections;
using FluentAssertions;
using Xunit;

namespace DG.Heuristic.Tests.Collections
{
    public class SubsetSumKnapsackTests
    {
        [Fact]
        public void PickClosest_NumbersAboveMax_CanBeFound()
        {
            var knapsack = new SubsetSumKnapsack(21);
            knapsack.Add(3000);

            knapsack.PickClosest(out int sum);

            sum.Should().Be(3000);
        }
    }
}
