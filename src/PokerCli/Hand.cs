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
    public class Hand
    {
        List<Card> _communityCards;

        List<Card> _holeCards;

        List<Card> _cards;


        public void AddHoleCards(IEnumerable<Card> holeCards)
        {
            Debug.Assert(holeCards.Count() == 2, $"Expected 2 hole cards.  Actual {holeCards.Count()}.");

            _communityCards = new List<Card>();
            _holeCards = new List<Card>(holeCards);
            RefreshCards();
        }

        public void AddFlopCards(IEnumerable<Card> flopCards)
        {
            _communityCards = new List<Card>(flopCards);
            RefreshCards();
        }

        public void AddTurnCard(Card turnCard) => AddCommunityCard(turnCard, 3);

        public void AddRiverCard(Card riverCard) => AddCommunityCard(riverCard, 4);


        // TODO: Why don't I have unit tests!?
        public IEnumerable<Card> GetBestHand()
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
                    return straight.straight;

                if(straight.isFlush)
                    return straight.straight;


                straightAvailable = true;
            }


            // four of a kind
            // TODO: add kicker
            if(fourOfAKind.Count() == 1)
                return _cards.Where(c => c.Rank == fourOfAKind.First().rank);


            // full house
            if(threeOfAKindSorted.Count() > 0 && pairsSorted.Count() > 0)
            {
                var threeRank = threeOfAKindSorted.First().rank;
                var pairRank = threeOfAKindSorted.First().rank;

                return _cards.Where(c => c.Rank == threeRank || c.Rank == pairRank);
            }


            // flush
            var greatestSuitByCount = cardsBySuit.OrderByDescending(cbs => cbs.Count).First();
            if(greatestSuitByCount.Count >= 5)
                return _cards.Where(c => c.Suit == greatestSuitByCount.suit).OrderByDescending(c => c.RankValue);


            // straight
            if(straightAvailable)
                return straight.straight;


            // three of a kind
            // TODO: add kickers
            if(threeOfAKindSorted.Count() > 0)
            {
                var threeRank = threeOfAKindSorted.First().rank;

                return _cards.Where(c => c.Rank == threeRank);
            }


            // two pairs
            // TODO: add kickers
            if(pairsSorted.Count() == 2)
            {
                var bestPair = pairsSorted.First().rank;
                var nextPair = pairsSorted.Skip(1).First().rank;

                return _cards.Where(c => c.Rank == bestPair || c.Rank == nextPair);
            }

            // airs
            // TODO: add kickers
            if(pairsSorted.Count() == 1)
            {
                var bestPair = pairsSorted.First().rank;

                return _cards.Where(c => c.Rank == bestPair);
            }



            // high card + kickers
            return _cards.OrderByDescending(c => c.RankValue).Take(5);




        }



        private void AddCommunityCard(Card card, int expectedCommunityCardCount)
        {
            Debug.Assert
                (
                    _communityCards.Count == expectedCommunityCardCount,
                    $"Expected {expectedCommunityCardCount} community cards.  Actual {_communityCards.Count}."
                )
            ;

            _communityCards.Add(card);
            RefreshCards();
        }

        private void RefreshCards() => _cards = _communityCards.Union(_holeCards).ToList();

        public IEnumerable<(Card firstCard, Card secondCard)> Pairs =>
            from card in _cards
            join cardRank in GetCardsByRank() on card.Rank equals cardRank.rank
            where cardRank.count == 2
            group card by card.RankValue into cardRankGroup
            select (cardRankGroup.First(), cardRankGroup.Last())
        ;

        public IEnumerable<(Card firstCard, Card secondCard)> FourOfAKind =>
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

        private bool TryGetStraight(out (Card[] straight, bool isFlush, bool isRoyal) bestStraight)
        {
            bestStraight =
                (
                    from straight in GetStraights()
                    orderby straight.isRoyal, straight.isFlush
                    select straight
                ).FirstOrDefault()
            ;

            return bestStraight.straight is not null;
        }

        private IEnumerable<(Card[] straight, bool isFlush, bool isRoyal)> GetStraights()
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

                            yield return (straight, isFlush, isRoyal);
                        }
                    }
        }
    }
}



/*
    ---------------------
    Hand value
    ---------------------
    10 - Royal flush*
     9 - Straight flush*
     8 - Four of a kind*            ++ kicker
     7 - Full house*
     6 - Flush
     5 - Straight
     4 - Three of a Kind            ++ kickers
     3 - Two Pairs                  ++ kicker
     2 - Pair                       ++ kickers
     1 - High card                  ++ kickers
    ---------------------
*/
