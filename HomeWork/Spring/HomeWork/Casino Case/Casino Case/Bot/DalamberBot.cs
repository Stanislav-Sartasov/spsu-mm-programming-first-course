using Casino_Case.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Bot
{
  public  class DalamberBot
    {
        private ColorBet BotColorBet = new ColorBet();
        public void Action(int AmountOfBets, int MinBid)
        {
            int BidOn = 0;
            int bid = MinBid;
            int BalanceInf = BetInfo.Balance;
            int wins = 0;
            for (int i = 0; i < AmountOfBets; i++)
            {
                BotColorBet.Betting(BidOn, bid);
                if (BalanceInf + bid == BetInfo.Balance)
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
                BalanceInf = BetInfo.Balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total Profit " + BetInfo.Profit);
            Console.WriteLine("Amount of bets " + BetInfo.AmountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }
    }
}
