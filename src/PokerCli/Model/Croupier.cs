using PokerCli.Config;
using PokerCli.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace PokerCli.Model
{
    public record AvailableBettingActions(bool CanCheck, bool CanCall, bool CanAllIn, bool CanRaise)
    {
        // you can always get out when the kitchen is too hot
        public bool CanFold => true;
    }

    public enum SelectedBettingAction
    {
        Check,
        Call,
        AllIn,
        Raise,
        Fold
    }


    public class Croupier
    {
        readonly CroupierConfig _croupierConfig;


        public Croupier(CroupierConfig croupierConfig) => (_croupierConfig) = (croupierConfig);


        public void InvokeBettingRound(Bank bank, List<Player> players, bool includeBlind)
        {
            Debug.Assert(players.Count >= 2, "Two or more players are required for a round of betting");

            decimal highestRaise = includeBlind ? _croupierConfig.BigBlind : 0;
            bool roundClosed = false;

            if(includeBlind)
            {
                bank.CreditPot(players[0], _croupierConfig.SmallBlind);
                bank.CreditPot(players[1], _croupierConfig.BigBlind);
            }


            while( ! roundClosed )
            {
                foreach(var player in players.Skip(includeBlind ? 0 : 2))
                {
                    // all in
                    if(player.Balance == 0)
                        break;

                    // only possible if highest bet is 0 (in current round)
                    if(CanCheck(player))
                        break;

                    // only possible if highest bet > 0 (in current round)
                    if(CanCall(player))
                        break;

                    // only when balance > 0 and < highest bid
                    if(CanAllIn(player))
                        break;

                    // balance > highest bid
                    if(CanRaise(player))
                        break;

                    // ([check] || [call]), [raise] || fold
                    // ( check || call || all-in ) [raise] fold


                    char bettingAction = '\0';
                    while(bettingAction is not 'c' or 'r' or 'f')
                        bettingAction = Console.ReadKey(false).KeyChar.ToLower();


                    switch (bettingAction)
                    {
                        case 'c':
                            if(highestRaise > 0)
                                bank.CreditPot(player,highestRaise);
                            break;
                        case 'r':
                            break;
                        case 'f':
                            players.Remove(player);
                            break;
                        default:
                            break;
                    }
                }


                highestRaise = 0;
            }


            // for each player
                // do you want to call/check/raise




            // # bet (check, call, raise, or fold)



        }


        private AvailableBettingActions GetAvailableBettingActions(Player player, decimal highestRaise) =>
            new AvailableBettingActions
            (
                CanCheck: highestRaise == 0,
                CanCall: highestRaise > 0 && player.Balance >= highestRaise,
                CanAllIn: highestRaise > 0 && player.Balance < highestRaise,
                CanRaise: player.Balance > highestRaise
            )
        ;

        private SelectedBettingAction GetSelectedBettingAction(AvailableBettingActions availableBettingActions)
        {



            void PrintPrompt()
            {
                var str = new StringBuilder("Place your bet: ");

                if(availableBettingActions.CanCheck)
                    str.Append("[C]heck | ");

                if(availableBettingActions.CanCall)
                    str.Append("[C]all | ");

                if(availableBettingActions.CanAllIn)
                    str.Append("[A]ll In | ");

                if(availableBettingActions.CanRaise)
                    str.Append("[R]aise | ");

                // you can always fold
                str.Append("[F]old");


                Console.WriteLine(str.ToString());
            }

            char GetUserSelection()
            {

            }
        }
    }
}
