/*
    ---------------------
    Hand value
    ---------------------
    10 - Royal flush
     9 - Straight flush
     8 - Four of a kind             ++ kicker
     7 - Full house
     6 - Flush
     5 - Straight
     4 - Three of a Kind            ++ kickers
     3 - Two Pairs                  ++ kicker
     2 - Pair                       ++ kickers  {200}
     1 - High card                  ++ kickers  {highest}{}{}{}{lowest}
    ---------------------
*/
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli
{
    public enum HandValue
    {
        HightCard,
        Pair,
        TwoPairs,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }


    public class Hand
    {
        readonly List<Card> _cards = new ();


        public Hand()
        { }

        public Hand(IEnumerable<Card> cards) => _cards.AddRange(cards);


        public void AddCard(Card card) => _cards.Add(card);

        public void AddCards(IEnumerable<Card> cards) => _cards.AddRange(cards);

        public void Clear() => _cards.Clear();

        public BestHand GetBestHand()
        {
            var cardsByRank = GetCardsByRank();
            var cardsBySuit = GetCardsBySuit();
            var pairsSorted = cardsByRank.Where(cbr => cbr.count == 2).OrderByDescending(cbr => cbr.rankValue);
            var threeOfAKindSorted = cardsByRank.Where(cbr => cbr.count == 3).OrderByDescending(cbr => cbr.rankValue);
            var fourOfAKind = cardsByRank.Where(cbr => cbr.count == 4);
            bool straightAvailable = false;


            // royal and straight flush
            if(TryGetStraight(out var straight))
            {
                if(straight.isRoyal)
                    return new BestHand(HandValue.RoyalFlush, straight.straight);

                if(straight.isFlush)
                    return new BestHand(HandValue.StraightFlush, straight.straight);

                straightAvailable = true;
            }


            // four of a kind
            if(fourOfAKind.Count() == 1)
            {
                var hand = AddKickers(_cards.Where(c => c.Rank == fourOfAKind.First().rank), 1);
                return new BestHand(HandValue.FourOfAKind, hand);
            }

            // full house a
            if(threeOfAKindSorted.Count() == 2)
            {
                var threeRank = threeOfAKindSorted.First().rank;
                var twoRank = threeOfAKindSorted.Last().rank;
                var threeOfAKind = _cards.Where(c => c.Rank == threeRank);
                var pair = _cards.Where(c => c.Rank == twoRank).Take(2);

                return new BestHand(HandValue.ThreeOfAKind, threeOfAKind.Union(pair));
            }


            // full house b
            if(threeOfAKindSorted.Count() == 1 && pairsSorted.Count() > 0)
            {
                var threeRank = threeOfAKindSorted.First().rank;
                var pairRank = pairsSorted.First().rank;
                var hand = _cards.Where(c => c.Rank == threeRank || c.Rank == pairRank);

                return new BestHand(HandValue.ThreeOfAKind, hand);
            }


            // flush
            var greatestSuitByCount = cardsBySuit.OrderByDescending(cbs => cbs.Count).First();
            if(greatestSuitByCount.Count >= 5)
            {
                var hand = _cards.Where(c => c.Suit == greatestSuitByCount.suit).OrderByDescending(c => c.RankValue);
                return new BestHand(HandValue.Flush, hand);
            }


            // straight
            if(straightAvailable)
                return new BestHand(HandValue.Straight, straight.straight);


            // three of a kind
            if(threeOfAKindSorted.Count() > 0)
            {
                var threeRank = threeOfAKindSorted.First().rank;
                var hand = AddKickers(_cards.Where(c => c.Rank == threeRank), 2);

                return new BestHand(HandValue.ThreeOfAKind, hand);
            }


            // two pairs
            if(pairsSorted.Count() == 2)
            {
                var bestPair = pairsSorted.First().rank;
                var nextPair = pairsSorted.Skip(1).First().rank;
                var hand = AddKickers(_cards.Where(c => c.Rank == bestPair || c.Rank == nextPair), 1);

                return new BestHand(HandValue.TwoPairs, hand);
            }

            // pair
            if(pairsSorted.Count() == 1)
            {
                var bestPair = pairsSorted.First().rank;
                var hand = AddKickers(_cards.Where(c => c.Rank == bestPair), 3);

                return new BestHand(HandValue.Pair, hand);
            }



            // high card + 4 kickers, which is the 5 highest cards by rank value
            return new BestHand(HandValue.HightCard, _cards.OrderByDescending(c => c.RankValue).Take(5));



            IEnumerable<Card> AddKickers(IEnumerable<Card> cards, int kickerCount) =>
                cards.Union
                (
                    _cards.Except(cards).OrderBy(c => c.Suit).ThenBy(c => c.RankValue).Take(kickerCount)
                )
            ;
        }


        private IEnumerable<(Card firstCard, Card secondCard)> Pairs =>
            from card in _cards
            join cardRank in GetCardsByRank() on card.Rank equals cardRank.rank
            where cardRank.count == 2
            group card by card.RankValue into cardRankGroup
            select (cardRankGroup.First(), cardRankGroup.Last())
        ;

        private IEnumerable<(Card firstCard, Card secondCard)> FourOfAKind =>
            from card in _cards
            join cardRank in GetCardsByRank() on card.Rank equals cardRank.rank
            where cardRank.count == 4
            group card by card.RankValue into cardRankGroup
            select (cardRankGroup.First(), cardRankGroup.Last())
        ;

        private IEnumerable<(CardRank rank, int rankValue, int count)> GetCardsByRank() =>
            from card in _cards
            group card by new {rank = card.Rank, rankValue = card.RankValue} into cardRankGroup
            select (cardRankGroup.Key.rank, cardRankGroup.Key.rankValue, cardRankGroup.Count())
        ;

        private IEnumerable<(CardSuit suit, int Count)> GetCardsBySuit() =>
            (
                from card in _cards
                group card by card.Suit into cardSuitGroup
                select (cardSuitGroup.Key, cardSuitGroup.Count())
            )
        ;

        private bool TryGetStraight(out (Card[] straight, Card highCard, bool isFlush, bool isRoyal) bestStraight)
        {
            bestStraight =
                (
                    from straight in GetStraights()
                    orderby straight.isRoyal, straight.isFlush, straight.highCard.RankValue descending
                    select straight
                ).FirstOrDefault()
            ;

            return bestStraight.straight is not null;
        }

        private IEnumerable<(Card[] straight, Card highCard, bool isFlush, bool isRoyal)> GetStraights()
        {
            // hand is sorted by value.
            // duplicate rank values (ace, 2, 3, etc) are merged.
            // where possible, the favourite suit is retained.
            // where not, it does not matter which suit we retain.
            // this simplifies detection of straight and royal flushes.
            // ex: ♥2, ♣2, ♥3, ♥4, ♥5, ♥6 ♥7 becomes ♥2, ♥3, ♥4, ♥5, ♥6 ♥7
            var favouriteSuit = GetCardsBySuit().OrderByDescending(cbs => cbs.Count).First().suit;
            var sortedHand =
                (
                    from card in _cards
                    group card by card.RankValue into cardRankGroup
                    orderby cardRankGroup.Key
                    select cardRankGroup.OrderBy(c => c.Suit == favouriteSuit ? -1 : (int)c.Suit).First()
                ).ToArray()
            ;


            // you cannot build a straight with fewer than 5 cards
            if(_cards.Count() >= 5)
                for(var startCard = 0; startCard <= _cards.Count() - 5; startCard++)
                    for(var i = 1; i < 5; i++)
                    {
                        var lastCard = sortedHand[startCard + i - 1];
                        var currentCard = sortedHand[startCard + i];

                        if(lastCard.RankValue + 1 != currentCard.RankValue)
                            break;

                        // we found one!
                        if(i == 4)
                        {
                            var straight = sortedHand[new Range(startCard, startCard + 5)];
                            var isFlush = straight.GroupBy(c => c.Suit).Count() == 1;
                            var isRoyal = isFlush && straight.Last().Rank == CardRank.Ace;
                            var highCard = straight.OrderBy(c => c.RankValue).Last();

                            yield return (straight, highCard, isFlush, isRoyal);
                        }
                    }
        }
    }
}
