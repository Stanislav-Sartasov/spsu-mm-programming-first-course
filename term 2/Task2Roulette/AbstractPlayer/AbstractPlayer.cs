using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractPlayer
{
    public abstract class AbstractPlayer
    {
       
        public string name;
        public int wallet;
        public int currentBet;
        public string betName;
        public abstract void Bet();

    }
}
