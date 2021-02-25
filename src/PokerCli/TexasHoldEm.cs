using PokerCli.Model;
using PokerCli.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli
{
    public class TexasHoldEm
    {
        readonly GameView _gameView;

        readonly Bank _bank;

        readonly BettingManager _bettingManager;

        readonly Hand _hand;

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


        public TexasHoldEm(
            GameView gameView,
            Bank bank,
            BettingManager bettingManager,
            Hand hand,
            Deck deck,
            List<Player> players
        )
        {
            _gameView = gameView;
            _bank = bank;
            _bettingManager = bettingManager;
            _hand = hand;
            _deck = deck;
            _players = players;
        }


        public void Play()
        {
            // var dealer = 0;
            var bigBlind = 10M;
            var smallBlind = 5M;


            foreach(var player in _players)
                _bank.InitialisePlayerBalance(player);


            // each iteration is a round
            // we continue playing rounds until only one player is left
            while (playersLeftInGame() > 1)
            {
                List<Card> communityCards = new();

                _deck.Shuffle();

                // blind
                _bank.PlaceBet(_players[1], smallBlind);
                _bank.PlaceBet(_players[2], bigBlind);
                _gameView.Redraw(_bank, _players, communityCards);

                foreach(var player in _players)
                {
                    player.SetHoleCards(_deck);
                    _gameView.Redraw(_bank, _players, communityCards);
                }


                foreach(var (_, bettingRound) in _bettingRounds)
                {
                    communityCards.AddRange(_deck.Deal(bettingRound.communityCardsRequired));
                    _bettingManager.InvokeBettingRound(_bank, _players, bigBlind);
                    _gameView.Redraw(_bank, _players, communityCards);
                }

                foreach(var player in _players.Where(p => p.IsOut == false))
                {
                    // need a better api here
                    _hand.Clear();
                    _hand.AddCards(communityCards);
                    _hand.AddCards(player.HoleCards);
                    player.BestHand = _hand.GetBestHand();
                }

                _gameView.Redraw(_bank, _players, communityCards);
                Console.ReadKey();
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


        private Player GetNextDealer() =>
            _players.Where(p => p.IsDealer).FirstOrDefault()?.NextPlayer()
            ?? _players.First()
        ;
    }
}
