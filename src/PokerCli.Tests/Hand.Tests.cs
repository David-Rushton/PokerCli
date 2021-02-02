using PokerCli;
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PokerCli.Tests
{
    public class HandTests
    {
        const char _suitHearts = '♥';

        const char _suitSpades = '♠';

        const char _suitDiamonds = '♦';

        const char _suitClubs = '♣';


        [Theory]
        [InlineData(new [] { "T♥", "J♥", "Q♥", "K♥", "A♥" }, new [] { "A♥", "K♥", "Q♥", "J♥", "T♥", "4♣", "7♦" }, HandValue.RoyalFlush)]
        [InlineData(new [] { "T♥", "J♥", "Q♥", "K♥", "A♥" }, new [] { "K♥", "Q♥", "J♥", "T♥", "4♣", "7♦", "A♥" }, HandValue.RoyalFlush)]
        public void Hand_ShouldReturnRoyalFlush(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "9♥", "T♥", "J♥", "Q♥", "K♥" }, new [] { "9♥", "K♥", "Q♥", "J♥", "T♥", "4♣", "7♦" }, HandValue.StraightFlush)]
        [InlineData(new [] { "9♥", "T♥", "J♥", "Q♥", "K♥" }, new [] { "K♥", "Q♥", "J♥", "T♥", "4♣", "8♥", "9♥" }, HandValue.StraightFlush)]
        public void Hand_ShouldReturnStraightFlush(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;


        [Theory]
        [InlineData(new [] { "A♥", "A♣", "A♦", "A♠", "7♠" }, new [] { "A♣", "A♥", "A♦", "A♠", "3♦", "7♠", "2♥" }, HandValue.FourOfAKind)]
        [InlineData(new [] { "A♥", "A♣", "A♦", "A♠", "7♠" }, new [] { "2♥", "7♠", "A♥", "A♣", "A♦", "A♠", "3♦" }, HandValue.FourOfAKind)]
        public void Hand_ShouldReturnFourOfAKind(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "7♥", "7♦", "7♠", "2♣", "2♥" }, new [] { "7♥", "7♦", "7♠", "2♣", "2♥", "A♦", "8♠" }, HandValue.FullHouse)]
        [InlineData(new [] { "7♥", "7♦", "7♠", "2♣", "2♥" }, new [] { "7♥", "7♦", "7♠", "8♠", "2♣", "A♦", "2♥" }, HandValue.FullHouse)]
        [InlineData(new [] { "7♥", "7♦", "7♠", "2♣", "2♥" }, new [] { "7♥", "7♦", "7♠", "2♥", "2♦", "2♠", "9♣" }, HandValue.FullHouse)]
        [InlineData(new [] { "2♦", "2♠", "2♣", "7♣", "7♥" }, new [] { "2♦", "2♣", "2♠", "7♥", "7♣", "A♠", "3♥" }, HandValue.FullHouse)]
        public void Hand_ShouldReturnFullHouse(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "K♣", "J♣", "9♣", "6♣", "2♣" }, new [] { "K♣", "J♣", "9♣", "6♣", "6♦", "7♥", "2♣" }, HandValue.Flush)]
        [InlineData(new [] { "8♣", "7♣", "6♣", "5♣", "3♣" }, new [] { "8♣", "7♣", "6♣", "5♣", "3♣", "2♣", "2♦" }, HandValue.Flush)]
        public void Hand_ShouldReturnFlush(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "2♣", "3♣", "4♦", "5♠", "6♥" }, new [] { "5♠", "6♥", "A♥", "3♦", "2♣", "3♣", "4♦" }, HandValue.Straight)]
        [InlineData(new [] { "4♥", "5♣", "6♣", "7♦", "8♠" }, new [] { "T♥", "A♠", "5♣", "4♥", "7♦", "6♣", "8♠" }, HandValue.Straight)]
        public void Hand_ShouldReturnStraight(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "8♣", "8♥", "8♠", "T♥", "4♥" }, new [] { "8♣", "2♠", "8♥", "8♠", "4♥", "3♠", "T♥" }, HandValue.ThreeOfAKind)]
        [InlineData(new [] { "9♣", "9♥", "9♠", "T♥", "4♥" }, new [] { "9♣", "2♠", "9♠", "4♥", "T♥", "3♠", "9♥" }, HandValue.ThreeOfAKind)]
        public void Hand_ShouldReturnThreeOfAKind(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "5♥", "5♦", "3♠", "3♥", "7♦" }, new [] { "6♠", "5♥", "5♦", "3♠", "3♥", "7♦", "2♠" }, HandValue.TwoPairs)]
        [InlineData(new [] { "9♠", "9♦", "5♠", "5♥", "3♣" }, new [] { "2♦", "9♠", "9♦", "5♠", "5♥", "3♣", "2♣" }, HandValue.TwoPairs)]
        public void Hand_ShouldReturnTwoPair(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "6♦", "6♥", "9♥", "7♣", "4♠" }, new [] { "6♦", "6♥", "9♥", "7♣", "4♠", "3♥", "2♣" }, HandValue.Pair)]
        [InlineData(new [] { "A♠", "A♣", "T♦", "8♦", "5♠" }, new [] { "A♣", "A♠", "T♦", "8♦", "5♠", "4♣", "2♣" }, HandValue.Pair)]
        public void Hand_ShouldReturnPair(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "Q♥", "J♠", "T♥", "8♠", "6♠" }, new [] { "Q♥", "6♠", "2♣", "8♠", "T♥", "J♠", "3♦" }, HandValue.HightCard)]
        [InlineData(new [] { "K♠", "J♠", "9♦", "7♥", "5♦" }, new [] { "7♥", "5♦", "3♠", "9♦", "J♠", "K♠", "2♥" }, HandValue.HightCard)]
        public void Hand_ShouldReturnHighCard(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;


        private void TestHandActualMatchesExpectations(string[] expectedCards, string[] allCards, HandValue expectedValue)
        {
            var hand = new Hand(GetCards(allCards));
            var actualHand = hand.GetBestHand();
            var expectedHand = new BestHand(expectedValue, GetCards(expectedCards).ToList());

            Assert.Equal(expectedHand.Value, expectedValue);
            Assert.Equal(expectedHand, actualHand);
        }

        private static IEnumerable<Card> GetCards(string[] cards)
        {
            foreach (var card in cards)
                yield return new Card(GetRank(card[0]), GetSuit(card[1]));


            CardSuit GetSuit(char suit) =>
                (suit) switch
                {
                    _suitHearts     => CardSuit.Hearts,
                    _suitClubs      => CardSuit.Clubs,
                    _suitDiamonds   => CardSuit.Diamonds,
                    _suitSpades     => CardSuit.Spades,

                    _ => throw new Exception($"Suit not supported: {suit}")
                }
            ;

            CardRank GetRank(char rank) =>
                new CardRank(GetRankSymbol(rank))
            ;

            CardRankSymbol GetRankSymbol(char rank) =>
                (rank) switch
                {

                    '2' => CardRankSymbol.Two,
                    '3' => CardRankSymbol.Three,
                    '4' => CardRankSymbol.Four,
                    '5' => CardRankSymbol.Five,
                    '6' => CardRankSymbol.Six,
                    '7' => CardRankSymbol.Seven,
                    '8' => CardRankSymbol.Eight,
                    '9' => CardRankSymbol.Nine,
                    'T' => CardRankSymbol.Ten,
                    'J' => CardRankSymbol.Jack,
                    'Q' => CardRankSymbol.Queen,
                    'K' => CardRankSymbol.King,
                    'A' => CardRankSymbol.Ace,

                    _ => throw new Exception($"Rank not supported: {rank}")
                }
            ;
        }
    }
}
