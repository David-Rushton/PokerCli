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
        [Theory]
        [InlineData(new [] { "TH", "JH", "QH", "KH", "AH" }, new [] { "AH", "KH", "QH", "JH", "TH", "4C", "7D" }, HandValue.RoyalFlush)]
        [InlineData(new [] { "TH", "JH", "QH", "KH", "AH" }, new [] { "KH", "QH", "JH", "TH", "4C", "7D", "AH" }, HandValue.RoyalFlush)]
        public void Hand_ShouldReturnRoyalFlush(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "9H", "TH", "JH", "QH", "KH" }, new [] { "9H", "KH", "QH", "JH", "TH", "4C", "7D" }, HandValue.StraightFlush)]
        [InlineData(new [] { "9H", "TH", "JH", "QH", "KH" }, new [] { "KH", "QH", "JH", "TH", "4C", "8H", "9H" }, HandValue.StraightFlush)]
        public void Hand_ShouldReturnStraightFlush(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;


        [Theory]
        [InlineData(new [] { "AH", "AC", "AD", "AS", "7S" }, new [] { "AC", "AH", "AD", "AS", "3D", "7S", "2H" }, HandValue.FourOfAKind)]
        [InlineData(new [] { "AH", "AC", "AD", "AS", "7S" }, new [] { "2H", "7S", "AH", "AC", "AD", "AS", "3D" }, HandValue.FourOfAKind)]
        public void Hand_ShouldReturnFourOfAKind(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "7H", "7D", "7S", "2C", "2H" }, new [] { "7H", "7D", "7S", "2C", "2H", "AD", "8S" }, HandValue.FullHouse)]
        [InlineData(new [] { "7H", "7D", "7S", "2C", "2H" }, new [] { "7H", "7D", "7S", "8S", "2C", "AD", "2H" }, HandValue.FullHouse)]
        [InlineData(new [] { "7H", "7D", "7S", "2C", "2H" }, new [] { "7H", "7D", "7S", "2H", "2D", "2S", "9C" }, HandValue.FullHouse)]
        [InlineData(new [] { "2D", "2S", "2C", "7C", "7H" }, new [] { "2D", "2C", "2S", "7H", "7C", "AS", "3H" }, HandValue.FullHouse)]
        public void Hand_ShouldReturnFullHouse(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "KC", "JC", "9C", "6C", "2C" }, new [] { "KC", "JC", "9C", "6C", "6D", "7H", "2C" }, HandValue.Flush)]
        [InlineData(new [] { "8C", "7C", "6C", "5C", "3C" }, new [] { "8C", "7C", "6C", "5C", "3C", "2C", "2D" }, HandValue.Flush)]
        public void Hand_ShouldReturnFlush(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "2C", "3C", "4D", "5S", "6H" }, new [] { "5S", "6H", "AH", "3D", "2C", "3C", "4D" }, HandValue.Straight)]
        [InlineData(new [] { "4H", "5C", "6C", "7D", "8S" }, new [] { "TH", "AS", "5C", "4H", "7D", "6C", "8S" }, HandValue.Straight)]
        public void Hand_ShouldReturnStraight(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "8C", "8H", "8S", "TH", "4H" }, new [] { "8C", "2S", "8H", "8S", "4H", "3S", "TH" }, HandValue.ThreeOfAKind)]
        [InlineData(new [] { "9C", "9H", "9S", "TH", "4H" }, new [] { "9C", "2S", "9S", "4H", "TH", "3S", "9H" }, HandValue.ThreeOfAKind)]
        public void Hand_ShouldReturnThreeOfAKind(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "5H", "5D", "3S", "3H", "7D" }, new [] { "6S", "5H", "5D", "3S", "3H", "7D", "2S" }, HandValue.TwoPairs)]
        [InlineData(new [] { "9S", "9D", "5S", "5H", "3C" }, new [] { "2D", "9S", "9D", "5S", "5H", "3C", "2C" }, HandValue.TwoPairs)]
        public void Hand_ShouldReturnTwoPair(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "6D", "6H", "9H", "7C", "4S" }, new [] { "6D", "6H", "9H", "7C", "4S", "3H", "2C" }, HandValue.Pair)]
        [InlineData(new [] { "AS", "AC", "TD", "8D", "5S" }, new [] { "AC", "AS", "TD", "8D", "5S", "4C", "2C" }, HandValue.Pair)]
        public void Hand_ShouldReturnPair(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
            TestHandActualMatchesExpectations(expectedCards, allCards, expectedValue)
        ;

        [Theory]
        [InlineData(new [] { "QH", "JS", "TH", "8S", "6S" }, new [] { "QH", "6S", "2C", "8S", "TH", "JS", "3D" }, HandValue.HightCard)]
        [InlineData(new [] { "KS", "JS", "9D", "7H", "5D" }, new [] { "7H", "5D", "3S", "9D", "JS", "KS", "2H" }, HandValue.HightCard)]
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
                    'H' => CardSuit.Hearts,
                    'C' => CardSuit.Clubs,
                    'D' => CardSuit.Diamonds,
                    'S' => CardSuit.Spades,

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
