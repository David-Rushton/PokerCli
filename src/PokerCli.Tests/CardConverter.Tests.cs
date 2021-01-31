using PokerCli;
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PokerCli.Tests
{
    public class CardConverterTests
    {
        [Fact]
        public void CardConverter_ShouldConvertAndBackAgainWithoutChange()
        {
            // converter is a 2 way lookup.
            // this test ensures te lookup returns the sane vakue both ways.
            // example: "2♦" == Card (2 of Diamonds) == "2♦".
            var converter = new CardCoverter();

            foreach(var suit in Enum.GetValues<CardSuit>())
                foreach(var rank in Enum.GetValues<CardRank>())
                {
                    var expectedCard = new Card(suit, rank);
                    var prettyPrint = converter.ConverToPrettyPrint(expectedCard);
                    var actualCard = converter.ConverToCard(prettyPrint);

                    Assert.Equal(expectedCard, actualCard);
                }
        }
    }
}
