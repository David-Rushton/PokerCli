using PokerCli;
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PokerCli.Tests
{
    public class HandTest
    {
        public static IEnumerable<Object[]> RoyalFlush()
        {
            var royalFlush = GetCards( new []{"TH", "JH", "QH", "KH", "AH"} );

            yield return new object[] { GetCards( new []{"AH", "KH", "QH", "JH", "TH", "4C", "7D"} ), royalFlush };
            yield return new object[] { GetCards( new []{"KH", "QH", "JH", "TH", "4C", "7D", "AH"} ), royalFlush };
        }

        public static IEnumerable<Object[]> StraightFlush()
        {
            var straightFlush = GetCards( new []{"TH", "JH", "QH", "KH", "AH"} );

            yield return new object[] { GetCards( new []{"9H", "KH", "QH", "JH", "TH", "4C", "7D"} ), straightFlush };
            yield return new object[] { GetCards( new []{"KH", "QH", "JH", "TH", "4C", "7D", "9H"} ), straightFlush };
        }

        public static IEnumerable<Object[]> FourOfAKind()
        {
            var fourOfAKind = GetCards( new []{"AH", "AC", "AD", "AS"} );

            yield return new object[] { GetCards( new []{"AH", "AC", "AD", "AS", "3D", "7S"} ), fourOfAKind };
            yield return new object[] { GetCards( new []{"7S", "KH", "QH", "JH", "TH", "3D"} ), fourOfAKind };
        }

        [Theory]
        [MemberData(nameof(RoyalFlush))]
        [MemberData(nameof(StraightFlush))]
        [MemberData(nameof(FourOfAKind))]
        public void Hand_ReturnsBestHand(IEnumerable<Card> allCards, IEnumerable<Card> expectedHand)
        {
            var hand = new Hand(allCards);
            var actualHand = hand.GetBestHand();

            // TODO: Implement compare
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
                    _   => (CardRank)((int)rank)
                }
            ;
        }
    }
}
