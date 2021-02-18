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
    public class AiBettingAction : IBettingAction
    {
        public (bettingAction BettingAction, decimal Stake) GetBet(Player player, decimal highestRaise)
        {
            throw new NotImplementedException();
        }
    }
}
