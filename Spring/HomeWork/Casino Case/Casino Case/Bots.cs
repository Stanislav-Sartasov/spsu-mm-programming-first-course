using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
   public class Bots
    {
        protected Game botGame = new Game();
        public void setBalance(int m)
        {
            botGame.player.balance = m;
        }

        protected int bid;
        protected int bidOn;
        protected int balanceInf;
       public void DalamberBot(int amountOfBets,int minBid)
        {
            bidOn = 0;
            bid = minBid;
            balanceInf = botGame.player.balance;
            int wins = 0;
            for (int i = 0; i < amountOfBets; i++) 
            {
                botGame.colorBet(bidOn, bid);
                if (balanceInf + bid == botGame.player.balance) 
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
                balanceInf = botGame.player.balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total profit " + botGame.player.profit);
            Console.WriteLine("Amount of bets " + botGame.player.amountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }

        public void MartingaleBot (int amountOfBets, int minBid)
        {
            bidOn = 0;
            bid = minBid;
            balanceInf = botGame.player.balance;
            int wins = 0;
            for (int i = 0; i < amountOfBets; i++)
            {
                botGame.parityBet(bidOn, bid);
                if (balanceInf + bid == botGame.player.balance)
                {

                    if (bidOn == 0)
                        bidOn = 1;
                    else
                        bidOn = 0;
                    bid = minBid;
                    wins++;
                }
                else
                {
                    bid *= 2;
                }
                balanceInf = botGame.player.balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total profit " + botGame.player.profit);
            Console.WriteLine("Amount of bets " + botGame.player.amountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }
    }
}
