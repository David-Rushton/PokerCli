using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // new TexasHoldEm().Play(new Deck(), new Player("player-1"), new Player("player-2"), new HandAssessor());


            // test case (that would fail today)
            //   Community: ♥Q, ♣K, ♣8, ♦10, ♣6
            //   Hole:      ♥9, ♦J
            //   Best hand: ♣8, ♥9, ♦10, ♦J, ♥Q
            // where is ♣K!?



            var deck = new Deck();
            var communityCards = new List<Card>(deck.Deal(5));
            var holeCards = new List<Card>(deck.Deal(2));

            var hand = new Hand();
            hand.AddHoleCards(holeCards);
            hand.AddFlopCards(communityCards.Take(3));
            hand.AddTurnCard(communityCards.Skip(3).Take(1).First());
            hand.AddRiverCard(communityCards.Skip(4).Take(1).First());


            var bestHand = hand.GetBestHand();

            Console.WriteLine($"Community: {string.Join(", ", communityCards)}");
            Console.WriteLine($"Hole:      {string.Join(", ", holeCards)}");
            Console.WriteLine($"Best hand: {string.Join(", ", bestHand)}");
        }
    }
}
