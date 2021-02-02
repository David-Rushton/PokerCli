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
            var playerOne = bank.GetPlayerBalances().First();

            Assert.Equal(10, playerOne.balance);
        }

        [Fact]
        public void Bank_ShouldDebitPlayer()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);

            Assert.True(bank.TryCreditPot(players[0], 5));
            Assert.Equal(5, bank.GetPlayerBalances().First().balance);
        }

        [Fact]
        public void Bank_ShouldNotDebitPlayer_WhenPlayerHasInsufficientFunds()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);

            Assert.False(bank.TryCreditPot(players[0], 20));
            Assert.Equal(10, bank.GetPlayerBalances().First().balance);
        }

        [Fact]
        public void Bank_ShouldNotCreditPot_WhenPlayerHasInsufficientFunds()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);
            bank.TryCreditPot(players[0], 20);

            Assert.Equal(0, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldCreditPot_WhenDebitingPlayer()
        {
            var (bank, players) = SetupTest(countOfPlayers: 1, playerStartingBalance: 10);

            bank.TryCreditPot(players[0], 5);
            Assert.Equal(5, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldCreditPot_WhenDebitingPlayers()
        {
            var (bank, players) = SetupTest(countOfPlayers: 2, playerStartingBalance: 10);

            bank.TryCreditPot(players[0], 5);
            bank.TryCreditPot(players[1], 5);

            Assert.Equal(10, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldCreditPotToWinningPlayer()
        {
            var (bank, players) = SetupTest(countOfPlayers: 2, playerStartingBalance: 10);

            bank.TryCreditPot(players[0], 2);
            bank.TryCreditPot(players[1], 2);
            bank.CreditPlayerWithPot(players[0]);

            var playerBalances = bank.GetPlayerBalances().ToArray();

            Assert.Equal(2, playerBalances.Length);
            Assert.Equal(12, playerBalances[0].balance);
            Assert.Equal(8, playerBalances[1].balance);
            Assert.Equal(0, bank.Pot);
        }

        [Fact]
        public void Bank_ShouldSplitPotEqually()
        {
            var (bank, players) = SetupTest(countOfPlayers: 2, playerStartingBalance: 10);

            bank.TryCreditPot(players[0], 2);
            bank.TryCreditPot(players[1], 2);
            bank.CreditPlayersWithSplitPot(players);

            var playerBalances = bank.GetPlayerBalances().ToArray();

            Assert.Equal(2, playerBalances.Length);
            Assert.Equal(10, playerBalances[0].balance);
            Assert.Equal(10, playerBalances[1].balance);
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
                bank.RegisterPlayer(players[i]);
            }

            return (bank, players);
        }
    }
}
