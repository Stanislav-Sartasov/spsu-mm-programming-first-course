using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Bots
    {
        protected Game BotGame = new Game();
        public void SetBalance(int m)
        {
            BotGame.balance = m;
        }
    }


}
