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
            int bet = 512;
            wallet = 4000;
            int correntWallet = wallet;
            for (int i = 0; i < 400; i++)
            {
                if (wallet > 0)
                {
                    if (bet > wallet)
                    {
                        while (bet > wallet)
                            bet /= 2;
                        
                    }
                    wallet = gameProcess.Payout(wallet, "Odd", bet);
                    if (correntWallet > wallet) bet *= 2;
                    else bet = 16;
                    correntWallet = wallet;
                }
            }
        }
    }
}