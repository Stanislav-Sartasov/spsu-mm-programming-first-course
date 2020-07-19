using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Bot
{
  public  class DalamberBot : Bots
    {
        public void Action(int amountOfBets, int minBid)
        {
            bidOn = 0;
            bid = minBid;
            balanceInf = botGame.balance;
            int wins = 0;
            for (int i = 0; i < amountOfBets; i++)
            {
                botGame.ColorBet(bidOn, bid);
                if (balanceInf + bid == botGame.balance)
                {

                    if (bidOn == 0)
                        bidOn = 1;
                    else
                        bidOn = 0;
                    if ((bid - minBid) >= minBid)
                        bid -= minBid;
                    wins++;
                }
                else
                {
                    bid += minBid;
                }
                balanceInf = botGame.balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total profit " + botGame.profit);
            Console.WriteLine("Amount of bets " + botGame.amountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }
    }
}
