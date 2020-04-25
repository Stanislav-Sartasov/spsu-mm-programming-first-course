using System;
using System.Collections.Generic;
using System.Text;
using Action;

namespace AbstractPlayer
{
    public class BotOliver : AbstractPlayer
    {
        
        GameProcess gameProcess = new GameProcess();
        
        public override void Bet()
        {
            wallet = 4000;
            int fixedBet = wallet / 400;
            for (int i = 0; i < 400; i++)
            {
                if (wallet > 0)
                {
                    wallet = gameProcess.Payout(wallet, "0", fixedBet);
                }
            }
        }
    }
}
