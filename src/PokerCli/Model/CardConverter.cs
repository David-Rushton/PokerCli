using System;
using System.Diagnostics;


namespace PokerCli.Model
{
    public class CardCoverter
    {
        const char _suitHearts = '♥';

        const char _suitSpades = '♠';

        const char _suitDiamonds = '♦';

        const char _suitClubs = '♣';


        public Card ConverToCard(string prettyPrint)
        {
            Debug.Assert(prettyPrint.Length == 2, $"Cannot convert {prettyPrint} to card.");

            var suit = (prettyPrint[1]) switch
            {
                _suitHearts     => CardSuit.Hearts,
                _suitSpades     => CardSuit.Spades,
                _suitDiamonds   => CardSuit.Diamonds,
                _suitClubs      => CardSuit.Clubs,
                _               => throw new Exception($"Cannot convert to card.  Suit not recognised: {prettyPrint[1]}")
            };

            var rank = (prettyPrint[0]) switch
            {
                '2' => CardRank.Two,
                '3' => CardRank.Three,
                '4' => CardRank.Four,
                '5' => CardRank.Five,
                '6' => CardRank.Six,
                '7' => CardRank.Seven,
                '8' => CardRank.Eight,
                '9' => CardRank.Nine,
                'T' => CardRank.Ten,
                'J' => CardRank.Jack,
                'Q' => CardRank.Queen,
                'K' => CardRank.King,
                'A' => CardRank.Ace,
                _   => throw new Exception($"Cannot convert to card.  Rank not recognised: {prettyPrint[0]}")
            };

            return new Card(suit, rank);
        }

        public string ConverToPrettyPrint(Card card)
        {
            var suit = (card.Suit) switch
            {
                CardSuit.Hearts     => _suitHearts,
                CardSuit.Spades     => _suitSpades,
                CardSuit.Diamonds   => _suitDiamonds,
                CardSuit.Clubs      => _suitClubs,
                _                   => throw new Exception($"Cannot convert from card.  Suit not recognised: {card.Suit}.")
            };

            var rank = (card.Rank) switch
            {
                CardRank.Two    => '2',
                CardRank.Three  => '3',
                CardRank.Four   => '4',
                CardRank.Five   => '5',
                CardRank.Six    => '6',
                CardRank.Seven  => '7',
                CardRank.Eight  => '8',
                CardRank.Nine   => '9',
                CardRank.Ten    => 'T',
                CardRank.Jack   => 'J',
                CardRank.Queen  => 'Q',
                CardRank.King   => 'K',
                CardRank.Ace    => 'A',
                _               => throw new Exception($"Cannot convert from card.  Rank not recognised: {card.Rank}.")
            };

            return $"{rank}{suit}";
        }
    }
}
