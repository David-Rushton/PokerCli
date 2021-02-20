using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli
{
    public class TexasHoldEm
    {
        readonly Bank _bank;

        readonly BettingManager _bettingManager;

        readonly Deck _deck;

        readonly List<Player> _players;

        readonly SortedList<int, (string Name, int communityCardsRequired)> _bettingRounds = new()
            {
                { 0, ("Pre-Flop",   0) },
                { 1, ("Flop",       3) },
                { 2, ("Turn",       1) },
                { 3, ("River",      1) },
                { 4, ("Showdown",   0) }
            }
        ;


        public TexasHoldEm(Bank bank, BettingManager bettingManager, Deck deck, List<Player> players) =>
            (_bank, _bettingManager, _deck, _players) = (bank, bettingManager, deck, players)
        ;


        public void Play()
        {
            var dealer = 0;
            var bigBlind = 10M;
            var smallBlind = 5M;


            // each iteration is a round
            // we continue playing rounds until only one player is left
            while (playersLeftInGame() > 1)
            {
                List<Card> communityCards = new();

                _deck.Shuffle();

                // blind
                _bank.PlaceBet(_players[1], smallBlind);
                _bank.PlaceBet(_players[2], bigBlind);

                _players.ForEach(p => p.SetHoleCards(_deck));


                foreach(var (_, bettingRound) in _bettingRounds)
                {
                    communityCards.AddRange(_deck.Deal(bettingRound.communityCardsRequired));
                    _bettingManager.InvokeBettingRound(_bank, _players, bigBlind);
                }
            }


            // playerOne.Hand.Clear();
            // playerTwo.Hand.Clear();


            // select dealer
            // # small and big blind
            // # deal players
            // playerOne.Hand = new List<Card>(deck.Deal(2));
            // playerTwo.Hand = new List<Card>(deck.Deal(2));

            // # bet (check, call, raise, or fold)
            // # deal flop
            // var communityCards = new List<Card>(deck.Deal(3));

            // # bet (check, call, raise, or fold)
            // # deal turn
            // communityCards.Add(deck.Deal(1).First());

            // # bet (check, call, raise, or fold)
            // # deal river
            // communityCards.Add(deck.Deal(1).First());

            // # bet (check, call, raise, or fold)
            // # win and lose, or draw


            int playersLeftInGame() => _players.Count(p => p.Balance > 0);
        }
    }
}
