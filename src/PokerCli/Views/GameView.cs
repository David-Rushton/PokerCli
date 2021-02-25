using System;
using System.Collections.Generic;
using System.Linq;
using PokerCli.Model;


namespace PokerCli.Views
{
    public class GameView
    {
        const string EscapeCode = "\u001b";




        public GameView()
        {

        }



        public void Redraw(Bank bank, List<Player> players, List<Card> communityCards)
        {
            Console.Clear();

            Console.WriteLine($"Pot: {bank.Pot}");

            Console.Write("Community cards: ");
            foreach(var card in communityCards)
                Console.Write($"{card.PrettyPrint()} ");

            Console.WriteLine();

            foreach(var player in players)
            {
                var playerState = player.IsOut ? "out" : "in";
                Console.WriteLine($"\n{player.Name}");
                Console.WriteLine($"Balance: {player.Balance}");
                Console.WriteLine($"Bet: {player.Bet}");
                Console.WriteLine($"State: {playerState}");

                if(player.BestHand is not null)
                {
                    Console.Write($"Best hand: {player.BestHand.Value} (");
                    foreach(var card in player.BestHand.Cards)
                        Console.Write($"{card.PrettyPrint()} ");

                    Console.WriteLine(")");
                }

                var isFacedown = player.IsHuman == false && player.BestHand is null;
                Console.Write("Hand: ");
                foreach(var card in player.HoleCards)
                    Console.Write($"{card.PrettyPrint(isFacedown)} ");

                Console.WriteLine();
            }
        }
    }
}
