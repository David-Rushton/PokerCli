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
    public class PlayerBettingAction : BettingActionBase
    {
        readonly IConsoleReader _consoleReader;


        public PlayerBettingAction(IConsoleReader consoleReader) => (_consoleReader) = (consoleReader);


        public override (BettingAction BettingAction, decimal Stake) GetBet(Player player, decimal highestRaise)
        {
            decimal stake = 0M;
            var availableBettingActions = base.GetAvailableBettingActions(player.Balance, highestRaise).ToDictionary
                (
                    k => k.key,
                    v => (v.prompt, v.bettingAction)
                )
            ;


            var flattenPrompt = string.Join(", ", availableBettingActions.Select(aba => aba.Value.prompt));
            var prompt = $"Available actions: {flattenPrompt}";
            var bettingAction = GetPlayerResponse(prompt, availableBettingActions.Keys.ToList());


            // check and call both match the current highest raise.
            // this may be zero, which is ok.
            if(bettingAction == 'c')
                stake = highestRaise;


            if(bettingAction == 'r')
                stake = GetPlayerResponse(highestRaise, player.Balance);


            return
                (
                    BettingAction: availableBettingActions[bettingAction].bettingAction,
                    Stake: stake
                )
            ;
        }


        private char GetPlayerResponse(string prompt, List<char> validResponses)
        {
            Console.Write(prompt);

            var response = '\0';
            do
            {
                response = _consoleReader.ReadKey().KeyChar.ToLower();
            } while ( ! validResponses.Contains(response) );

            return response;
        }

        private decimal GetPlayerResponse(decimal min, decimal max)
        {
            Console.Write($"Place your bet between {min.ToString("N0")} and {max.ToString("N0")}:");

            var response = 0M;
            do
            {
                var input = _consoleReader.ReadLine();
                if(decimal.TryParse(input, out response))
                    response = -1;

            } while (response < min && response > max);

            return response;
        }
    }
}
