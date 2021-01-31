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


        public IEnumerable<Card> Deal(int cardsRequested)
        {
            if(cardsRequested > RemainingCards)
                throw new Exception($"Cannot deal {cardsRequested} card(s).  Deck contains {RemainingCards} card(s).");

            for(var i = 0; i < cardsRequested; i++)
                yield return NextCard();
        }

        public Deck Shuffle() => RefillDeck();


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
        private Deck RefillDeck()
        {
            _deck.Clear();

            for(var i = 0; i < 52; i++)
                _deck.Add(new Card(GetCardRank(i), GetSuit(i)));


            CardSuit GetSuit(int idx) =>
                (idx / 13) switch
                {
                    0 => CardSuit.Hearts,
                    1 => CardSuit.Spades,
                    2 => CardSuit.Diamonds,
                    3 => CardSuit.Clubs,

                    _ => throw new Exception("Cannot generator card suit.  Unexpected seed value.")
                }
            ;


            Debug.Assert(_deck.Count == 52, $"After a shuffle the deck should contain 52 cards, not {_deck.Count}.");
            return this;


            CardRank GetCardRank(int idx) =>
                new CardRank(GetCardRankSymbol(idx))
            ;

            CardRankSymbol GetCardRankSymbol(int idx)
            {
                var rankValue = ((idx % 13) + 2);
                return (rankValue) switch
                {
                     2 => CardRankSymbol.Two,
                     3 => CardRankSymbol.Three,
                     4 => CardRankSymbol.Four,
                     5 => CardRankSymbol.Five,
                     6 => CardRankSymbol.Six,
                     7 => CardRankSymbol.Seven,
                     8 => CardRankSymbol.Eight,
                     9 => CardRankSymbol.Nine,
                    10 => CardRankSymbol.Ten,
                    11 => CardRankSymbol.Jack,
                    12 => CardRankSymbol.Queen,
                    13 => CardRankSymbol.King,
                    14 => CardRankSymbol.Ace,

                    _ => throw new Exception("Cannot generator card rank.  Unexpected seed value")
                };
            }
        }
    }
}
