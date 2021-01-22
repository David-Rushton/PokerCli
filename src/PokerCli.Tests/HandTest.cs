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
            var straightFlush = GetCards( new []{"9H", "TH", "JH", "QH", "KH"} );

            yield return new object[] { GetCards( new []{"9H", "KH", "QH", "JH", "TH", "4C", "7D"} ), straightFlush };
            yield return new object[] { GetCards( new []{"KH", "QH", "JH", "TH", "4C", "7D", "9H"} ), straightFlush };
        }

        public static IEnumerable<Object[]> FourOfAKind()
        {
            var fourOfAKindWithKicker = GetCards( new []{"AH", "AC", "AD", "AS", "7S"} );

            yield return new object[] { GetCards( new []{"AH", "AC", "AD", "AS", "3D", "7S"} ), fourOfAKindWithKicker };
            yield return new object[] { GetCards( new []{"7S", "AH", "AC", "AD", "AS", "3D"} ), fourOfAKindWithKicker };
        }

        public static IEnumerable<Object[]> FullHouse()
        {
            var fullHouse = GetCards( new []{"7H", "7D", "7S", "2C", "2H"} );

            yield return new object[] { GetCards( new []{"7H", "7D", "7S", "2C", "2H", "AD", "8S"} ), fullHouse };
            yield return new object[] { GetCards( new []{"7H", "7D", "7S", "8S", "2C", "AD", "2H"} ), fullHouse };
            yield return new object[] { GetCards( new []{"7H", "7D", "7S", "2H", "2D", "2S", "9C"} ), fullHouse };
        }


        [Theory]
        [MemberData(nameof(RoyalFlush))]
        [MemberData(nameof(StraightFlush))]
        [MemberData(nameof(FourOfAKind))]
        [MemberData(nameof(FullHouse))]
        public void Hand_ReturnsBestHand(IEnumerable<Card> allCards, IEnumerable<Card> expectedHand)
        {
            var hand = new Hand(allCards);
            var actualHand = SortHand(hand.GetBestHand());

            // TODO: Implement compare
            Assert.Equal(SortHand(expectedHand), actualHand);


            IEnumerable<Card> SortHand(IEnumerable<Card> cards) =>
                from card in cards
                orderby card.RankValue descending, card.Suit
                select card
            ;
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
