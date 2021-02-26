using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace PokerCli.Model
{
    public class Player : IEquatable<Player>
    {
        static int _idSeed;

        static Dictionary<int, Player> _players = new();


        public Player(string name) : this(name, false)
        { }

        public Player(string name, bool isHuman) : this()
        {
            name = Name;
            isHuman = IsHuman;
        }

        public Player()
        {
            Id = _idSeed++;
            IsDealer = Id == 0;

            _players.Add(Id, this);
        }


        public int Id { get; private set; }

        public string Name { get; init; } = $"Player #{_idSeed + 1}";

        public decimal Balance { get; set; }

        public decimal Bet { get; set; }

        public bool IsHuman { get; init; } = false;

        public bool HasFolded { get; set; }

        public bool IsDealer { get; private set; }

        public bool IsOut => Balance == 0M && Bet == 0M;

        public List<Card> HoleCards { get; private set; } = new List<Card>();

        public BestHand? BestHand { get; set; }


        public void SetHoleCards(Deck deck) => HoleCards = deck.Deal(2).ToList();

        public void SelectAsDealer()
        {
            var currentDealerIds = _players.Where(p => p.Value.IsDealer).Select(p => p.Value.Id);
            foreach(var dealerId in currentDealerIds)
                _players[dealerId].IsDealer = false;

            this.IsDealer = true;
        }

        public Player? NextPlayer() =>
            (
                from playerKvp in _players
                let id = playerKvp.Key
                let player = playerKvp.Value
                where player.Id != this.Id & player.IsOut == false
                orderby player.Id > this.Id, player.Id
                select player
            ).FirstOrDefault()
        ;

        public bool Equals(Player? other) => other?.Id == this.Id;
    }
}
