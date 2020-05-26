using System;
using System.Collections.Generic;
using System.Text;
using Action;

namespace AbstractPlayer
{
    public class BotStepan : AbstractPlayer
    {
        GameProcess gameProcess = new GameProcess();
        public override void Bet()
        {
            currentBet = 512;
            wallet = 4000;
            int correntWallet = wallet;
            for (int i = 0; i < 400; i++)
            {
                if (wallet > 0)
                {
                    if (currentBet > wallet)
                    {
                        while (currentBet > wallet)
                            currentBet /= 2;
                        
                    }
                    bool result = gameProcess.IsWin(1, 0);
                    RoundResult(result, gameProcess.GetCoefficient(1));
                    if (correntWallet > wallet) currentBet *= 2;
                    else currentBet = 16;
                    correntWallet = wallet;
                }
            }
        }
    }
}