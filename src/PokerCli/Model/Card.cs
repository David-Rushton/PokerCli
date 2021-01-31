using System;


namespace PokerCli.Model
{
    public record Card(CardRank Rank, CardSuit Suit)
    {
        const char _suitHearts = '♥';

        const char _suitSpades = '♠';

        const char _suitDiamonds = '♦';

        const char _suitClubs = '♣';


        public string PrettyPrint() =>
            string.Format
            (
                "\u001b[1;{0}{1}{2}\u001b[0m",
                GetAnsiSuitColourCode(),
                ConvertFromRank(),
                ConvertFromSuit()
            )
        ;

        public override string ToString() =>
            string.Format
            (
                "{0}{1}",
                ConvertFromRank(),
                ConvertFromSuit()
            )
        ;


        // https://en.wikipedia.org/wiki/ANSI_escape_code
        private string GetAnsiSuitColourCode() =>
            this.Suit is CardSuit.Hearts or CardSuit.Diamonds ? "31m" : "36m"
        ;

        private char ConvertFromSuit() =>
            (this.Suit) switch
            {
                CardSuit.Hearts     => _suitHearts,
                CardSuit.Spades     => _suitSpades,
                CardSuit.Diamonds   => _suitDiamonds,
                CardSuit.Clubs      => _suitClubs,

                _ => throw new Exception($"Cannot convert from card.  Suit not recognised: {this.Suit}.")
            }
        ;

        private char ConvertFromRank() =>
            (this.Rank.Symbol) switch
            {
                CardRankSymbol.Two    => '2',
                CardRankSymbol.Three  => '3',
                CardRankSymbol.Four   => '4',
                CardRankSymbol.Five   => '5',
                CardRankSymbol.Six    => '6',
                CardRankSymbol.Seven  => '7',
                CardRankSymbol.Eight  => '8',
                CardRankSymbol.Nine   => '9',
                CardRankSymbol.Ten    => 'T',
                CardRankSymbol.Jack   => 'J',
                CardRankSymbol.Queen  => 'Q',
                CardRankSymbol.King   => 'K',
                CardRankSymbol.Ace    => 'A',

                _ => throw new Exception($"Cannot convert from card.  Rank not recognised: {this.Rank}.")
            }
        ;
    }
}
