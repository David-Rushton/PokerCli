using PokerCli;
using PokerCli.Config;
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PokerCli.Tests
{
    public class BankTests
    {
        [Fact]
        public void Bank_ShouldCreditPlayerWithStartingBalance()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);
            var playerOne = players[0];

            Assert.Equal(10, playerOne.Balance);
        }

        [Fact]
        public void Bank_ShouldDebitPlayer()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);
            var playerOne = players[0];

            bank.CreditPot(playerOne, 5);
            Assert.Equal(5, playerOne.Balance);
        }

        [Fact]
        public void Bank_ShouldNotCreditPot_WhenPlayerHasInsufficientFunds()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);

            // this should throw.
            // we can swallow this exception.
            // for this test; we only care about about the final pot balance.
            try
            {
                bank.CreditPot(players[0], 20);
            }
            catch
            {
                // noop
            }

            Assert.Equal(0, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldCreditPot_WhenDebitingPlayer()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);

            bank.CreditPot(players[0], 5);
            Assert.Equal(5, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldCreditPot_WhenDebitingPlayers()
        {
            var (bank, players) = SetupTest(countOfPlayers: 2, playerStartingBalance: 10);

            bank.CreditPot(players[0], 5);
            bank.CreditPot(players[1], 5);

            Assert.Equal(10, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldCreditPotToWinningPlayer()
        {
            var (bank, players) = SetupTest(countOfPlayers: 2, playerStartingBalance: 10);

            bank.CreditPot(players[0], 2);
            bank.CreditPot(players[1], 2);
            bank.CreditPlayerWithPot(players[0]);

            Assert.Equal(2, players.Length);
            Assert.Equal(12, players[0].Balance);
            Assert.Equal(8, players[1].Balance);
            Assert.Equal(0, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldSplitPotEqually()
        {
            var (bank, players) = SetupTest(countOfPlayers: 2, playerStartingBalance: 10);

            bank.CreditPot(players[0], 2);
            bank.CreditPot(players[1], 2);
            bank.CreditPlayersWithSplitPot(players);

            Assert.Equal(2, players.Length);
            Assert.Equal(10, players[0].Balance);
            Assert.Equal(10, players[1].Balance);
            Assert.Equal(0, bank.Pot);
        }


        private (Bank bank, Player[] players) SetupTest(int countOfPlayers, decimal playerStartingBalance)
        {
            var players = new Player[countOfPlayers];
            var bankConfig = new BankConfig { PlayerStartingBalance = playerStartingBalance };
            var bank = new Bank(bankConfig);

            for(var i = 0; i < countOfPlayers; i++)
            {
                players[i] = new Player($"P{i}");
                bank.InitialisePlayerBalance(players[i]);
            }

            return (bank, players);
        }
    }
}
