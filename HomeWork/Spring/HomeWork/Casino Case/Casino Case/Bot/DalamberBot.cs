using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Bot
{
  public  class DalamberBot : Bots
    {
        public void Action(int AmountOfBets, int MinBid)
        {
            int BidOn = 0;
            int bid = MinBid;
            int BalanceInf = BotGame.balance;
            int wins = 0;
            for (int i = 0; i < AmountOfBets; i++)
            {
                BotGame.ColorBet(BidOn, bid);
                if (BalanceInf + bid == BotGame.balance)
                {

                    if (BidOn == 0)
                        BidOn = 1;
                    else
                        BidOn = 0;
                    if ((bid - MinBid) >= MinBid)
                        bid -= MinBid;
                    wins++;
                }
                else
                {
                    bid += MinBid;
                }
                BalanceInf = BotGame.balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total profit " + BotGame.profit);
            Console.WriteLine("Amount of bets " + AmountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }
    }
}
