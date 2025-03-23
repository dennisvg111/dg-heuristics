using DG.Heuristic.Collections;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
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
            knapsack.Add(new CardWithValue("Hearts", 2));
            knapsack.Add(new CardWithValue("Spades", 9));
            knapsack.Add(new CardWithValue("Diamonds", 3));

            var cards = knapsack.PickClosest(out int sum);

            sum.Should().Be(21);
            cards.Count.Should().Be(3);
            cards.OrderBy(c => c).Should().BeEquivalentTo(new CardWithValue[]
            {
                new CardWithValue("Diamonds", 3),
                new CardWithValue("Clubs", 9),
                new CardWithValue("Spades", 9)
            });
        }

        [Fact]
        public void PickClosest_MultipleOptions_ReturnsFirstAdded()
        {
            var card4 = new CardWithValue("Clubs", 4);
            var card5 = new CardWithValue("Clubs", 5);
            var card9 = new CardWithValue("Clubs", 9);
            var knapsackA = new SubsetSumKnapsack<CardWithValue>(9);
            var knapsackB = new SubsetSumKnapsack<CardWithValue>(9);

            knapsackA.Add(new CardWithValue[] { card4, card5, card9 });
            knapsackB.Add(new CardWithValue[] { card9, card5, card4 });

            knapsackA.PickClosest(out int _)
                .Should().HaveCount(2)
                .And.Contain(card4)
                .And.Contain(card5);

            knapsackB.PickClosest(out int _)
                .Should().ContainSingle()
                .And.Contain(card9);
        }

        private class CardWithValue : IWeightedData, IEquatable<CardWithValue>, IComparable<CardWithValue>
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

            public int CompareTo(CardWithValue other)
            {
                if (other == null)
                {
                    return 0;
                }
                if (_value == other._value)
                {
                    return _name.CompareTo(other._name);
                }
                return _value.CompareTo(other._value);
            }


            public override string ToString()
            {
                return _name;
            }
        }
    }
}
