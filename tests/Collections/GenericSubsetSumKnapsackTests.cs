using DG.Heuristic.Collections;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DG.Heuristic.Tests.Collections
{
    public class GenericSubsetSumKnapsackTests
    {
        [Fact]
        public void PickClosest_Returns_Instances()
        {
            var knapsack = new SubsetSumKnapsack<CardWithValue>(21);
            knapsack.Add(new CardWithValue("Clubs", 4));
            knapsack.Add(new CardWithValue("Clubs", 9));
            knapsack.Add(new CardWithValue("Spades", 9));
            knapsack.Add(new CardWithValue("Diamonds", 3));
            knapsack.Add(new CardWithValue("Hearts", 2));

            var cards = knapsack.PickClosest(out int sum);

            sum.Should().Be(21);
            cards.Count.Should().Be(3);
            cards.Should().Contain(c => c.Name == "Spades 9")
                .And.NotContain(c => c.Name == "Clubs 4")
                .And.NotContain(c => c.Name == "Hearts 2");
        }


        private class CardWithValue : IWeightedData, IEquatable<CardWithValue>
        {
            private readonly string _name;
            private readonly int _value;

            public string Name => _name;

            public CardWithValue(string suit, int value)
            {
                _name = suit + " " + value;
                _value = value;
            }

            public int Weight => _value;

            public override bool Equals(object obj)
            {
                return obj is CardWithValue value &&
                       this.Equals(value);
            }

            public bool Equals(CardWithValue other)
            {
                return !(other is null) &&
                       _name == other._name;
            }

            public override int GetHashCode()
            {
                return -1125283371 + EqualityComparer<string>.Default.GetHashCode(_name);
            }
        }
    }
}
