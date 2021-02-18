using PokerCli;
using PokerCli.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;


namespace PokerCli.Tests
{
    public class PlayerBettingActionTests
    {
        [Fact]
        public void PlayerBettingAction_ShouldReturnHasFolded_WhenPlayerFolds()
        {
            var keys = new Queue<ConsoleKeyInfo>( new [] { new ConsoleKeyInfo('f', ConsoleKey.F, false, false, false) });
            var lines = new Queue<string>();
            var readerStub = new ConsoleFake(keys, lines);
            var playerBettingAction = new PlayerBettingAction(readerStub);
            var player = new Player("player-one") { Balance = 20M };
            // var stdIn = Console.In;

            // Console.SetIn(mockIn);

            var output = playerBettingAction.GetSomethingSomething(player, 5M);


            var expected = true;
            var actual = output.HasFolded;

            // Console.SetIn(stdIn);

            Assert.Equal(expected, actual);



            // using var reader = new StringReader("r10");
            // var playerBettingAction = new PlayerBettingAction();
            // var player = new Player("player_one") { Balance = 10.00M };
            // var originalIn = Console.In;

            // Console.SetIn(reader);
            // var actual = playerBettingAction.GetSomethingSomething(player, 0);
            // var expected = (PlayerStake: 10M, HasFolded: false);





            // Console.SetIn(originalIn);

            // Assert.Equal(expected, actual);

            //Console.SetIn()
        }



        private class ConsoleFake : IConsoleReader
        {
            readonly Queue<ConsoleKeyInfo> _keys;

            readonly Queue<string> _lines;


            public ConsoleFake(Queue<ConsoleKeyInfo> keys, Queue<string> lines) => (_keys, _lines) = (keys, lines);


            public ConsoleKeyInfo ReadKey() => ReadKey(false);

            public ConsoleKeyInfo ReadKey(bool intercept)
            {
                Debug.Assert(_keys.Count > 0, "There are no keys to read");
                return _keys.Dequeue();
            }

            public string ReadLine()
            {
                Debug.Assert(_lines.Count > 0, "There are no lines to read");
                return _lines.Dequeue();
            }
        }
    }
}
