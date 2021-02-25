using System;


namespace PokerCli.Model
{
    public record Card(CardRank Rank, CardSuit Suit)
    {
        const string TerminalEscapeCode = "\u001b";

        const string CardBack = "🂠";

        const char SuitHearts = '♥';

        const char SuitSpades = '♠';

        const char SuitDiamonds = '♦';

        const char SuitClubs = '♣';


        public string PrettyPrint(bool facedown) =>
            facedown
            ? $"{TerminalEscapeCode}[1;94m{CardBack}{TerminalEscapeCode}[0m"
            : PrettyPrint()
        ;

        public string PrettyPrint() =>
            string.Format
            (
                "{0}[1;{1}{2}{3}{4}[0m",
                TerminalEscapeCode,
                GetAnsiSuitColourCode(),
                ConvertFromRank(),
                ConvertFromSuit(),
                TerminalEscapeCode
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
                CardSuit.Hearts     => SuitHearts,
                CardSuit.Spades     => SuitSpades,
                CardSuit.Diamonds   => SuitDiamonds,
                CardSuit.Clubs      => SuitClubs,

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
