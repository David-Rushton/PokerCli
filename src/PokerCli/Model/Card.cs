using System;

namespace PokerCli.Model
{
    public enum CardSuit
    {
        Hearts,
        Spades,
        Diamonds,
        Clubs
    }


    public enum CardRank
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }


    public record Card(CardSuit Suit, CardRank Rank)
    {
        public int RankValue => (int)Rank;


        // TODO: 🤮 let's refactor this later.
        public string ToString(bool withFormat)
        {
            var shortRank = (Rank) switch
            {
                CardRank.Jack => "J",
                CardRank.Queen => "Q",
                CardRank.King => "K",
                CardRank.Ace => "A",
                _ => ((int)Rank).ToString()
            };

            // TODO: Fix odd whitespace here .
            var shortSuit = (Suit) switch
            {
                CardSuit.Hearts => "♥",
                CardSuit.Spades => "♠",
                CardSuit.Diamonds => "♦",
                _ => "♣"
            };


            return (shortSuit) switch
            {
                // TODO: Fix odd whitespace here
                "♥" => $"\u001b[1;31m{shortSuit}{shortRank}\u001b[0m",
                "♠" => $"\u001b[1;36m{shortSuit}{shortRank}\u001b[0m",
                "♦" => $"\u001b[1;7;31m{shortSuit}{shortRank}\u001b[0m",
                _   => $"\u001b[1;7;36m{shortSuit}{shortRank}\u001b[0m",
            };
        }

        public override string ToString()
        {
            var shortRank = (Rank) switch
            {
                CardRank.Jack => "J",
                CardRank.Queen => "Q",
                CardRank.King => "K",
                CardRank.Ace => "A",
                _ => ((int)Rank).ToString()
            };

            // TODO: Fix odd whitespace here
            var shortSuit = (Suit) switch
            {
                CardSuit.Hearts => "♥",
                CardSuit.Spades => "♠",
                CardSuit.Diamonds => "♦",
                _ => "♣"
            };


            return $"{shortRank}{shortSuit}";
        }
    }
}
