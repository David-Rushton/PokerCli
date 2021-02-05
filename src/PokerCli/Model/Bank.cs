using PokerCli.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli.Model
{
    public class Bank
    {
        readonly decimal _playerStartingBalance;


        public Bank(BankConfig bankConfig) =>
            (_playerStartingBalance) = (bankConfig.PlayerStartingBalance)
        ;


        public decimal Pot { get; private set; }


        public void InitialisePlayerBalance(Player player) =>
            player.Balance = _playerStartingBalance
        ;

        public void CreditPot(Player player, decimal amount)
        {
            Debug.Assert(player.Balance > amount, "You cannot bet what you do not have");

            DebitPlayerAccount(player, amount);
        }

        public void CreditPlayerWithPot(Player player) =>
            CreditPlayerAccount(player, Pot)
        ;

        public void CreditPlayersWithSplitPot(Player[] players)
        {
            var playersCount = players.Length;
            var playerShare = Pot / playersCount;

            foreach(var player in players)
                CreditPlayerAccount(player, playerShare);
        }


        private void CreditPlayerAccount(Player player, decimal amount) =>
            TransferFundsBetweenPlayerAndPot(player, amount)
        ;

        private void DebitPlayerAccount(Player player, decimal amount) =>
            TransferFundsBetweenPlayerAndPot(player, amount * -1)
        ;

        private void TransferFundsBetweenPlayerAndPot(Player player, decimal amount)
        {
            player.Balance += amount;
            Pot += amount * -1;
        }
    }
}
