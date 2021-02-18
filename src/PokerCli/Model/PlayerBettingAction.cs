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
    public class PlayerBettingAction : IBettingAction
    {
        readonly IConsoleReader _consoleReader;


        public PlayerBettingAction(IConsoleReader consoleReader) => (_consoleReader) = (consoleReader);


        public (bettingAction BettingAction, decimal Stake) GetBet(Player player, decimal highestRaise)
        {
            var availableBettingActions = new Dictionary<char, string>('f');
            decimal playerStake = 0;

            // can check
            if(highestRaise == 0)
                availableBettingActions.Add('c', "[C]heck");

            // can call
            if(highestRaise > 0 && player.Balance >= highestRaise)
                availableBettingActions.Add('c', "[C]all");

            // can all in
            if(highestRaise > 0 && player.Balance < highestRaise)
                availableBettingActions.Add('a', "[A]ll in");

            // can raise
            if(player.Balance > highestRaise)
                availableBettingActions.Add('r', "[R]aise");

            // you can always fold
            availableBettingActions.Add('f', "[F]old");




            var bettingAction = '\0';
            while( ! availableBettingActions.ContainsKey(bettingAction) )
                bettingAction = _consoleReader.ReadKey(false).KeyChar.ToLower();



            // check and call both match the current highest raise.
            // this may be zero, which is ok.
            if(bettingAction == 'c')
                playerStake = highestRaise;

            if(bettingAction == 'r')
            {
                do
                {
                    Console.WriteLine("how much?");0
                    var possibleRaise = _consoleReader.ReadLine();

                    if(decimal.TryParse(possibleRaise, out playerStake))
                    {
                        // noop
                    }
                } while( ! (playerStake > highestRaise && playerStake <= player.Balance) );
            }



            return
                (
                    PlayerStake: playerStake,
                    HasFolded: bettingAction == 'f' ? true : false
                )
            ;
        }
    }
}
