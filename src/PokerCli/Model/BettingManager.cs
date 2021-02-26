using PokerCli.Config;
using PokerCli.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace PokerCli.Model
{
    public class BettingManager
    {
        readonly BettingActionBase _playerBettingAction;

        readonly BettingActionBase _aiBettingAction;


        public BettingManager(BettingActionBase playerBettingAction, BettingActionBase aiBettingAction) =>
            (_playerBettingAction, _aiBettingAction) = (playerBettingAction, aiBettingAction)
        ;


        public void InvokeBettingRound(Bank bank, List<Player> players, decimal bigBlind)
        {
            var maxBet = bigBlind;

            do
            {
                foreach(var player in players.Where(p => p.HasFolded == false))
                {
                    var bet = GetBettingAction(player);
                    switch (bet.BettingAction)
                    {
                        case BettingAction.Check:
                            // no-op.
                            break;

                        case BettingAction.AllIn:
                            bank.PlaceBet(player, player.Balance);
                            maxBet = player.Balance;
                            break;

                        case BettingAction.Call:
                        case BettingAction.Raise:
                            bank.PlaceBet(player, bet.Stake);
                            maxBet = bet.Stake;
                            break;

                        case BettingAction.Fold:
                            player.HasFolded = true;
                            break;

                        default:
                            throw new Exception($"Betting action not supported: {bet.BettingAction}");
                    }
                }
            } while ( ! IsBettingRoundOver() );


            bool IsBettingRoundOver()
            {
                if(players.Where(p => p.IsOut == false || p.HasFolded == false).Count() is 1)
                    return true;

                // if everyone has bet the same amount the round is over.
                if( ! players.Exists(p => p.Bet < maxBet) )
                    return true;

                return false;
            }

            (BettingAction BettingAction, decimal Stake) GetBettingAction(Player player)
            {
                var bettingAction = player.IsHuman ? _playerBettingAction : _aiBettingAction;
                return bettingAction.GetBet(player, maxBet);
            }
        }
    }
}
