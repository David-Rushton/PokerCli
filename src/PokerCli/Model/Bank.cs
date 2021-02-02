using PokerCli.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli.Model
{
    public class Bank
    {
        readonly BankConfig _bankConfig;

        readonly Dictionary<string, BankAccount> _playerAccounts = new();


        public Bank(BankConfig bankConfig) => (_bankConfig) = (bankConfig);


        public decimal Pot { get; private set; }


        public IEnumerable<(string playerName, decimal balance)> GetPlayerBalances()
        {
            foreach(var (playerName, account) in _playerAccounts)
                yield return (playerName, account.Balance);
        }

        public void RegisterPlayer(Player player)
        {
            Debug.Assert( ! _playerAccounts.ContainsKey(player.Name), "Cannot create account.  Player is already registered.");

            _playerAccounts.Add(player.Name, new BankAccount(_bankConfig.PlayerStartingBalance));
        }

        public bool TryCreditPot(Player player, int amount)
        {
            if(_playerAccounts[player.Name].Balance < amount)
                return false;

            DebitPlayerAccount(player, amount);
            return true;
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
            var side = amount > 0 ? "credit" : "debit";
            Debug.Assert(_playerAccounts.ContainsKey(player.Name), $"Cannot {side} player {player.Name}.  Not registered with bank.");

            var account = _playerAccounts[player.Name];
            _playerAccounts[player.Name] = account with { Balance = account.Balance + amount };
            Pot += amount * -1;
        }
    }
}
