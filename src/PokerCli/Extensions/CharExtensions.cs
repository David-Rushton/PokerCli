using System;


namespace PokerCli.Extensions
{
    static public class CharExtensions
    {
        static public char ToLower(this char originalValue) => originalValue.ToString().ToLower()[0];
    }
}
