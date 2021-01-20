using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli
{
    public class TexasHoldEm
    {
        public void Play(Deck deck, Player playerOne, Player playerTwo, HandAssessor assessor)
        {
            #region round

            playerOne.Hand.Clear();
            playerTwo.Hand.Clear();
            ;

            // select dealer
            // # small and big blind
            // # deal players
            playerOne.Hand = new List<Card>(deck.Deal(2));
            playerTwo.Hand = new List<Card>(deck.Deal(2));

            // # bet (check, call, raise, or fold)
            // # deal flop
            var communityCards = new List<Card>(deck.Deal(3));

            // # bet (check, call, raise, or fold)
            // # deal turn
            communityCards.Add(deck.Deal(1).First());

            // # bet (check, call, raise, or fold)
            // # deal river
            communityCards.Add(deck.Deal(1).First());

            // # bet (check, call, raise, or fold)
            // # win and lose, or draw

            PrintRound();
            assessor.Assess(communityCards.Union(playerOne.Hand).ToList());

            #endregion



            void PrintRound()
            {
                Console.WriteLine("--- Round One --------------------");
                Console.WriteLine($"Community: {string.Join(", ", communityCards)}");
                Console.WriteLine($"Player one: {string.Join(", ", playerOne.Hand)}");
                Console.WriteLine($"Player two: {string.Join(", ", playerTwo.Hand)}\n");
            }
        }
    }
}
