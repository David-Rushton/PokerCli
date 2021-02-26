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

        readonly Dictionary<int, Player> _playersInPot = new();


        public Bank(BankConfig bankConfig) =>
            (_playerStartingBalance) = (bankConfig.PlayerStartingBalance)
        ;


        public decimal Pot { get; private set; }


        public void InitialisePlayerBalance(Player player) =>
            player.Balance = _playerStartingBalance
        ;

        public void PlaceBet(Player player, decimal wager)
        {
            if( ! _playersInPot.ContainsKey(player.Id) )
                _playersInPot.Add(player.Id, player);

            CreditPot(player, wager);
        }

        public void PayWinner(Player player)
        {
            ClearBetsInPot();
            CreditPlayerWithPot(player);
        }

        public void PayWinners(Player[] players)
        {
            ClearBetsInPot();
            CreditPlayersWithSplitPot(players);
        }


        private void ClearBetsInPot()
        {
            foreach(var (key, player) in _playersInPot)
            {
                player.Bet = 0;
                _playersInPot.Remove(key);
            }
        }

        private void CreditPot(Player player, decimal amount)
        {
            Debug.Assert(player.Balance > amount, "You cannot bet what you do not have");

            DebitPlayerAccount(player, amount);
        }

        private void CreditPlayerWithPot(Player player) =>
            CreditPlayerAccount(player, Pot)
        ;

        private void CreditPlayersWithSplitPot(Player[] players)
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
            player.Bet = amount * -1;
            player.Balance += amount;
            Pot += amount * -1;
        }
    }
}
