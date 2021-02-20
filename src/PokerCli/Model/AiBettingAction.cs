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
    public class AiBettingAction : BettingActionBase
    {
        public override (BettingAction BettingAction, decimal Stake) GetBet(Player player, decimal highestRaise)
        {
            var availableBettingActions = GetAvailableBettingActions(player.Balance, highestRaise);

            // todo: repace with something more challenging to play against.
            // this is just a placeholder.

            if(availableBettingActions.ToList().Exists(ba => ba.bettingAction is BettingAction.Call))
                return (BettingAction.Call, 0M);


            if(availableBettingActions.ToList().Exists(ba => ba.bettingAction is BettingAction.Check))
                return (BettingAction.Check,highestRaise);


            return (BettingAction.Fold, 0M);
        }
    }
}
