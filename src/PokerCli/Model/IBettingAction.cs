using System;
using System.Collections.Generic;
using System.Linq;


namespace PokerCli.Model
{
    public enum BettingAction
    {
        Check,
        Call,
        AllIn,
        Raise,
        Fold
    }


    public abstract class BettingActionBase
    {
        public abstract (BettingAction BettingAction, decimal Stake) GetBet(Player player, decimal highestRaise);


        protected IEnumerable<(char key, string prompt, BettingAction bettingAction)> GetAvailableBettingActions(
            decimal playerBalance,
            decimal highestRaise
        )
        {
            // check and call both have a key of 'c'.
            // it does not matter that they clash.
            // they are mutually exclusive.
            // in both cases the outcome is the same (player matches highest bet, which may be zero).


            // can all in.
            if(highestRaise > 0 && playerBalance < highestRaise)
                yield return ('a', "[A]ll in", BettingAction.AllIn);

            // can call.
            if(highestRaise > 0 && playerBalance >= highestRaise)
                yield return ('c', "[C]all", BettingAction.Call);

            // can check.
            if(highestRaise == 0)
                yield return ('c', "[C]heck", BettingAction.Check);

            // can raise.
            if(playerBalance > highestRaise)
                yield return ('r', "[R]aise", BettingAction.Raise);

            // you can always fold.
            yield return ('f', "[F]old", BettingAction.Fold);
        }
    }
}
