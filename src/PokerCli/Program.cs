using PokerCli.Config;
using PokerCli.Model;
using PokerCli.Views;
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

            Bootstrap().Play();
        }


        static TexasHoldEm Bootstrap()
        {
            var gameView = new GameView();
            var consoleReader = new ConsoleReader();
            var aiBettingAction = new AiBettingAction();
            var playerBettingAction = new PlayerBettingAction(consoleReader);
            var hand = new Hand();
            var bettingManager = new BettingManager(playerBettingAction, aiBettingAction);
            var bankConfig = new BankConfig() { PlayerStartingBalance = 100M };
            var bank = new Bank(bankConfig);
            var deck = new Deck();
            var players = new List<Player>
                {
                    new Player() { Name = "David", IsHuman = true },
                    new Player() { Name = "NPC #1" },
                    new Player() { Name = "NPC #2" }
                };


            return new TexasHoldEm(gameView, bank, bettingManager, hand, deck, players);
        }
    }
}
