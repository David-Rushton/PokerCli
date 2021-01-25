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
/*
        public static IEnumerable<Object[]> RoyalFlush()
        {
            var royalFlush = new BestHand(HandValue.RoyalFlush, GetCards( new []{"TH", "JH", "QH", "KH", "AH"} ));

            yield return new object[] { GetCards( new []{"AH", "KH", "QH", "JH", "TH", "4C", "7D"} ), royalFlush };
            yield return new object[] { GetCards( new []{"KH", "QH", "JH", "TH", "4C", "7D", "AH"} ), royalFlush };
        }

        public static IEnumerable<Object[]> StraightFlush()
        {
            var straightFlush = GetCards( new []{"9H", "TH", "JH", "QH", "KH"} );

            yield return new object[] { GetCards( new []{"9H", "KH", "QH", "JH", "TH", "4C", "7D"} ), straightFlush };
            yield return new object[] { GetCards( new []{"KH", "QH", "JH", "TH", "4C", "7D", "9H"} ), straightFlush };
        }

        public static IEnumerable<Object[]> FourOfAKind()
        {
        }


        [Theory]
        [MemberData(nameof(RoyalFlush))]
        public void Hand_ShouldReturnRoyalFlush(IEnumerable<Card> allCards, BestHand expectedHand)
        {
            var hand = new Hand(allCards);
            var actualHand = hand.GetBestHand();

            Assert.Equal(expectedHand.Value, HandValue.RoyalFlush);
            Assert.Equal(expectedHand, actualHand);
        }

        [Theory]
        [MemberData(nameof(StraightFlush))]
        // [MemberData(nameof(FourOfAKind))]
        // [MemberData(nameof(FullHouse))]
        public void Hand_ShouldReturnStraightFlush(IEnumerable<Card> allCards, BestHand expectedHand)
        {
            var hand = new Hand(allCards);
            var actualHand = hand.GetBestHand();

            Assert.Equal(expectedHand.Value, HandValue.StraightFlush);
            Assert.Equal(expectedHand, actualHand);
        }
*/

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
        [InlineData(new [] { "8C", "7C", "6C", "5C", "3C" }, new [] { "8C", "7C", "6C", "5C", "3C", "2C", "1C" }, HandValue.Flush)]
        public void Hand_ShouldReturnFlush(string[] expectedCards, string[] allCards, HandValue expectedValue) =>
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
                yield return new Card(GetSuit(card[1]), GetRank(card[0]));


            CardSuit GetSuit(char suit) =>
                (suit) switch
                {
                    'H' => CardSuit.Hearts,
                    'C' => CardSuit.Clubs,
                    'D' => CardSuit.Diamonds,
                    'S' => CardSuit.Spades,
                    _   => throw new Exception($"Suit not supported: {suit}")
                }
            ;

            CardRank GetRank(char rank) =>
                (rank) switch
                {
                    'A' => CardRank.Ace,
                    'K' => CardRank.King,
                    'Q' => CardRank.Queen,
                    'J' => CardRank.Jack,
                    'T' => CardRank.Ten,
                    _   => (CardRank)int.Parse(rank.ToString())
                }
            ;
        }
    }
}
