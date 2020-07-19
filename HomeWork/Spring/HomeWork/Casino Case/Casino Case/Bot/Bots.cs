using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Bots
    {
        protected Game botGame = new Game();
        public void SetBalance(int m)
        {
            botGame.balance = m;
        }

        protected int bid;
        protected int bidOn;
        protected int balanceInf;
    }


}
