using PokerCli;
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PokerCli.Tests
{
    public class DeckTests
    {
        [Fact]
        public void Deck_ShouldContainAllFiftyTwoCards()
        {
            var deck = new Deck().Deal(52).ToList();

            foreach(var suit in (CardSuit[])Enum.GetValues(typeof(CardSuit)))
                foreach(var rank in (CardRank[])Enum.GetValues(typeof(CardRank)))
                    Assert.Contains(new Card(suit, rank), deck);
        }

        [Fact]
        public void Deck_ShouldContainFiftyTwoDistinctCards_WhenOneDeckDealt()
        {
            var deck = new Deck();
            var cards =
                (
                    from card in deck.Deal(52)
                    group card by new { card.Suit, card.Rank } into cardGroup
                    select new { cardGroup.Key, Count = cardGroup.Count() }
                ).ToDictionary(k => k.Key, v => v.Count)
            ;


            Assert.Equal(1, cards.Min(c => c.Value));
            Assert.Equal(1, cards.Max(c => c.Value));
            Assert.Equal(52, cards.Count);
        }

        [Fact]
        public void Deck_ShouldContainFiftyTwoDistinctCards_WhenTwoDecksDealt()
        {
            var deck = new Deck();
            var cards =
                (
                    from card in deck.Deal(104)
                    group card by new { card.Suit, card.Rank } into cardGroup
                    select new { cardGroup.Key, Count = cardGroup.Count() }
                ).ToDictionary(k => k.Key, v => v.Count)
            ;


            Assert.Equal(2, cards.Min(c => c.Value));
            Assert.Equal(2, cards.Max(c => c.Value));
            Assert.Equal(52, cards.Count);
        }

        [Fact]
        public void Deck_ShouldReplenishAfterFiftyTwoCardsDealt()
        {
            var exception = Record.Exception(() => new Deck().Deal(53));
            Assert.Null(exception);
        }

        [Fact]
        public void Deck_ShouldBeRandom_WhenShuffled()
        {
            // a standard 52 card deck can produce 80,658 vigintillion combinations.
            // in theory two randomly shuffled desks are almost guaranteed to be different.
            // maybe one day this test will fail...?
            var deck = new Deck();
            var firstShuffle = string.Join(string.Empty, deck.Deal(52));
            var secondShuffle = string.Join(string.Empty, deck.Deal(52));

            Assert.NotEqual(firstShuffle, secondShuffle);
        }

    }
}
