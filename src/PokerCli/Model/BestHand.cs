using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace PokerCli.Model
{
    public record BestHand : IComparable
    {
        public BestHand(HandValue value, IEnumerable<Card> cards)
        {
            Debug.Assert(cards.Count() == 5, "A hand must be comprised of 5 cards");

            Value = value;
            Cards = cards.ToList();
            Score = GetScore();
        }


        public HandValue Value { get; init; }

        public int Score { get; init; }

        public List<Card> Cards { get; init; }


        // requires test
        // TODO: value is required in check
        public int CompareTo(object? other) =>
            (other as BestHand) switch
            {
                var o when o?.Score <  this.Score => -1,
                var o when o?.Score == this.Score =>  0,
                _                                 =>  1
            }
        ;

        // TODO: value is required in check
        public virtual bool Equals(BestHand? other) =>
            other?.Value == this.Value && other?.Score == this.Score
        ;

        // TODO: value is _maybe_ needed here.
        public override int GetHashCode() => (this.Value, this.Score).GetHashCode();

        public override string ToString() =>
            string.Format
            (
                "BestHand {{ Value = {0}, Score = {1}, Cards = {2} }}",
                Value,
                Score,
                string.Join(',', Cards)
            )
        ;


        private int GetScore()
        {
            int value = 0;
            int multiplier = 1;

            // each card in the hand contributes to the score.
            // rank of two is 2, three is 3 and so on.  Jack is 11, queen is 12, king is 13 and ace is 14.
            // rank is multiplied by car position.
            // last card is *least* significant.  it is multiplied by 1.
            // penultimate is next leaste.  it is multiplied by 100.
            // we add two zeros for each subsequent card.  the first card is multiplied by 100,000,000.

            // TODO: ahem.  we don't do this.
            // finally the hand value is multiplied by 10,000,000,000 and added to the result.

            // example.  pair with kicker: 3H 3D, AD, 10S, 5S =   20,303,141,005
            // hand value is assigned the highest multiplier to ensure a pair is never beaten by a lesser hand (high card).
            // multiplying by position ensures high value kickers cannot overwhelm the pair cards.
            foreach(var card in Cards.Reverse<Card>())
            {
                value += card.Rank.Value * multiplier;
                multiplier *= 100;
            }

            return value;
        }

    }
}
