using System;


namespace PokerCli.Model
{
    public record CardRank(CardRankSymbol Symbol)
    {
        public int Value =>
            (Symbol) switch
            {
                CardRankSymbol.Two      =>  2,
                CardRankSymbol.Three    =>  3,
                CardRankSymbol.Four     =>  4,
                CardRankSymbol.Five     =>  5,
                CardRankSymbol.Six      =>  6,
                CardRankSymbol.Seven    =>  7,
                CardRankSymbol.Eight    =>  8,
                CardRankSymbol.Nine     =>  9,
                CardRankSymbol.Ten      => 10,
                CardRankSymbol.Jack     => 11,
                CardRankSymbol.Queen    => 12,
                CardRankSymbol.King     => 13,
                CardRankSymbol.Ace      => 14,

                _ => throw new Exception($"Symbol not recognised: {Symbol}")
            }
        ;
    }
}
