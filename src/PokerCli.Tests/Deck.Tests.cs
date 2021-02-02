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
            // TODO: why do I **need** to materialise the view here?
            var deck = new Deck().Shuffle().Deal(52).ToList();

            foreach(var suit in Enum.GetValues<CardSuit>())
                foreach(var rank in Enum.GetValues<CardRankSymbol>())
                    Assert.Contains(new Card(new CardRank(rank), suit), deck);
        }

        [Fact]
        public void Deck_ShouldContainFiftyTwoDistinctCards_WhenOneDeckDealt()
        {
            var deck = new Deck().Shuffle();
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
        public void Deck_ShouldThrowOnFiftyThridDeal_AfterShuffle()
        {
            var deck = new Deck().Shuffle();

            deck.Deal(52).ToList();
            Assert.Throws<Exception>(() => deck.Deal(1).ToList());
        }

        [Fact]
        public void Deck_ShouldBeRandom_WhenShuffled()
        {
            // a standard 52 card deck can produce 80,658 vigintillion combinations.
            // in theory two randomly shuffled desks are almost guaranteed to be different.
            // maybe one day this test will fail...?
            var deck = new Deck();
            var firstShuffle = string.Join(string.Empty, deck.Shuffle().Deal(52).ToList());
            var secondShuffle = string.Join(string.Empty, deck.Shuffle().Deal(52).ToList());

            Assert.NotEqual(firstShuffle, secondShuffle);
        }

    }
}
