
/*
    ---------------------
    Hand value
    ---------------------
    10 - Royal flush
     9 - Straight flush
     8 - Four of a kind
     7 - Full house
     6 - Flush
     5 - Straight
     4 - Three of a Kind
     3 - Two Pairs
     2 - Pair
     1 - High card
    ---------------------
*/


using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli
{
    public class HandAssessor
    {
        public void Assess(List<Card> cards)
        {
            var highCard = cards.OrderByDescending(c => c.RankValue).First();
            var cardValueGroup =
                from card in cards
                group card by card.RankValue into cardGroup
                select new { Value = cardGroup.Key, Count = cardGroup.Count() }
            ;
            var cardSuitGroup =
                from card in cards
                group card by card.Suit into cardGroup
                select new { Value = cardGroup.Key, Count = cardGroup.Count() }
            ;

            var pairs = cardValueGroup.Where(cvg => cvg.Count == 2).Select(cvg => cvg.Value);
            var threes = cardValueGroup.Where(cvg => cvg.Count == 3).Select(cvg => cvg.Value);
            var fours = cardValueGroup.Where(cvg => cvg.Count == 4).Select(cvg => cvg.Value);


            TryGetStraight(out var straight);

/*
    ---------------------
    Hand value
    ---------------------
    ---------------------
*/

            Console.WriteLine($"--- Hand Assessment ---------------------");
            Console.WriteLine($"  10 - Royal flush: "); //straight
            Console.WriteLine($"   9 - Straight flush: "); //straight
            Console.WriteLine($"   8 - Four of a kind: { cardValueGroup.Where(cvg => cvg.Count == 4).Count() }");
            Console.WriteLine($"   7 - Full house: ");
            Console.WriteLine($"   6 - Flush: { cardSuitGroup.Where(csg => csg.Count == 5).Count() }");
            Console.WriteLine($"   5 - Straight: {  string.Join(',', straight) }"); //straight
            Console.WriteLine($"   4 - Three of a Kind: { string.Join(',', cards.Where(c => threes.Contains(c.RankValue))) }");
            Console.WriteLine($"   3 - Two Pairs: { string.Join(',', cards.Where(c => pairs.Contains(c.RankValue))) }");
            Console.WriteLine($"   2 - Pair: { string.Join(',', cards.Where(c => pairs.Contains(c.RankValue))) }");
            Console.WriteLine($"   1 - High card: { highCard }");





            bool TryGetStraight(out List<Card> straight)
            {
                var favourSuit =
                    (
                        from card in cards
                        group card by card.Suit into cardSuit
                        orderby cardSuit.Count() descending
                        select cardSuit.Key
                    ).First()
                ;
                // Cards are sorted by face value.
                // We take the favoured suit where possible.
                // This simplifies detecting royal/straight flushes.
                var sortedCards =
                    (
                        from card in cards
                        group card by card.RankValue into cardValue
                        select cardValue.OrderBy(c => c.Suit == favourSuit ? -1 : (int)c.Suit).First()
                    ).ToArray()
                ;
                (CardSuit suit, int sequentialCards) consecutiveSuit = (sortedCards.Take(1).First().Suit, 1);

                straight = new List<Card>(sortedCards.Take(1));

                for(var i = 1; i < sortedCards.Count(); i++)
                {
                    var lastCard = sortedCards[i - 1];
                    var currentCard = sortedCards[i];

                    if(lastCard.RankValue + 1 == currentCard.RankValue)
                    {
                        straight.Add(currentCard);

                        if(consecutiveSuit.suit == currentCard.Suit)
                            consecutiveSuit.sequentialCards++;
                        else
                            if(consecutiveSuit.sequentialCards < 5)
                                consecutiveSuit = (currentCard.Suit, 1);
                    }
                    else
                    {
                        straight.Clear();

                        // There aren't enough cards left to make a straight.
                        if(i > 3)
                            return false;
                    }



                }

                if(consecutiveSuit.sequentialCards >=5)
                {
                    straight = straight.Where(c => c.Suit == consecutiveSuit.suit).TakeLast(5).ToList();
                }


                // we did it.
                return true;
            }
        }



        // private IEnumerable<List<Card>> GetSequences(List<Card> cards)
        // {
        //     var favourSuit =
        //         (
        //             from card in cards
        //             group card by card.Suit into cardSuit
        //             orderby cardSuit.Count() descending
        //             select cardSuit.Key
        //         ).First()
        //     ;
        //     // Cards are sorted by face value.
        //     // We take the favoured suit where possible.
        //     // This simplifies detecting royal/straight flushes.
        //     var sortedCards =
        //         (
        //             from card in cards
        //             group card by card.RankValue into cardValue
        //             select cardValue.OrderBy(c => c.Suit == favourSuit ? -1 : (int)c.Suit).First()
        //         ).ToArray()
        //     ;

        //     var result = new List<Card>(sortedCards.First());

        //     foreach

        // }
    }
}
