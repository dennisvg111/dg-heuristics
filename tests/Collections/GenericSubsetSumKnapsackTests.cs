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
