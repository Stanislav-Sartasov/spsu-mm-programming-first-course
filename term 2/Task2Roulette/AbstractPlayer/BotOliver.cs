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
            currentBet = wallet / 400;
            for (int i = 0; i < 400; i++)
            {
                if (wallet > 0)
                {
                    RoundResult(gameProcess.Result(8, 0), gameProcess.GetCoefficient(8));
                }
            }
        }
    }
}
