using System;


namespace PokerCli.Model
{
    public interface IConsoleReader
    {
        public ConsoleKeyInfo ReadKey();

        public ConsoleKeyInfo ReadKey(bool intercept);

        public string? ReadLine();
    }


    public class ConsoleReader : IConsoleReader
    {
        public ConsoleKeyInfo ReadKey() => Console.ReadKey();

        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

        public string? ReadLine() => Console.ReadLine();
    }
}
