using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace PokerCli.Model
{
    public class Deck
    {
        readonly List<Card> _deck = new();


        public int RemainingCards => _deck.Count;


        public IEnumerable<Card> Deal(int numberOfCards)
        {
            for(var i = 0; i < numberOfCards; i++)
                yield return NextCard();
        }

        public void Shuffle() => RefillDeck();


        private Card NextCard()
        {
            if(_deck.Count == 0)
                throw new Exception("The deck is empty.  All cards have been dealt.");


            var nextCardIndex = new Random().Next(0, _deck.Count);
            var nextCard = _deck.Skip(nextCardIndex).First();
            _deck.Remove(nextCard);

            return nextCard;
        }

        // TODO: Why rebuild this time and time again?
        private void RefillDeck()
        {
            _deck.Clear();

            for(var i = 0; i < 52; i++)
                _deck.Add(new Card(GetSuit(i), GetRank(i)));


            CardSuit GetSuit(int idx) =>
                (idx / 13) switch
                {
                    0 => CardSuit.Hearts,
                    1 => CardSuit.Spades,
                    2 => CardSuit.Diamonds,
                    3 => CardSuit.Clubs,
                    _ => throw new Exception("Cannot generator card suit.  Unexpected seed value")
                }
            ;

            CardRank GetRank(int idx)
            {
                var rankValue = ((idx % 13) + 2);
                return (rankValue) switch
                {
                    11 => CardRank.Jack,
                    12 => CardRank.Queen,
                    13 => CardRank.King,
                    14 => CardRank.Ace,
                    _  => (CardRank)rankValue
                };
            }
        }
    }
}
