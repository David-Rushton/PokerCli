using PokerCli;
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PokerCli.Tests
{
    public class HandValueTests
    {
        [Theory]
        [InlineData(CardRank.Two, 2)]
        [InlineData(CardRank.Three, 3)]
        [InlineData(CardRank.Four, 4)]
        [InlineData(CardRank.Five, 5)]
        [InlineData(CardRank.Six, 6)]
        [InlineData(CardRank.Seven, 7)]
        [InlineData(CardRank.Eight, 8)]
        [InlineData(CardRank.Nine, 9)]
        [InlineData(CardRank.Ten, 10)]
        [InlineData(CardRank.Jack, 11)]
        [InlineData(CardRank.Queen, 12)]
        [InlineData(CardRank.King, 13)]
        [InlineData(CardRank.Ace, 14)]
        public void HandValue_ShouldMatchCardRank_WithEnumValue(CardRank rank, int expectedValue) =>
            Assert.Equal((int)rank, expectedValue)
        ;
    }
}
