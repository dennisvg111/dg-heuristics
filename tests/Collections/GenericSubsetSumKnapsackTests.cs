using DG.Heuristic.Collections;
using FluentAssertions;
using Xunit;

namespace DG.Heuristic.Tests.Collections
{
    public class GenericSubsetSumKnapsackTests
    {
        [Fact]
        public void PickClosest_Returns_Instances()
        {
            var knapsack = new SubsetSumKnapsack<CardWithValue>(21);
            knapsack.Add(new CardWithValue("Clubs 4", 4));
            knapsack.Add(new CardWithValue("Clubs 9", 9));
            knapsack.Add(new CardWithValue("Spades 9", 9));
            knapsack.Add(new CardWithValue("Diamonds 3", 3));

            var cards = knapsack.PickClosest(out int sum);

            sum.Should().Be(21);
            cards.Count.Should().Be(3);
            cards.Should().Contain(c => c.Name == "Spades 9");
            cards.Should().NotContain(c => c.Name == "Clubs 4");
        }


        private class CardWithValue : IWeightedData
        {
            private readonly string _name;
            private readonly int _value;

            public string Name => _name;
            public int Value => _value;

            public CardWithValue(string name, int value)
            {
                _name = name;
                _value = value;
            }

            public int Weight => _value;
        }
    }
}
