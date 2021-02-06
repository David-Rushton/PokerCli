using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace PokerCli.Model
{
    public class Player
    {
        public Player(string name) => (Name) = (name);


        public string Name { get; init; }

        public decimal Balance { get; set; }
    }
}
