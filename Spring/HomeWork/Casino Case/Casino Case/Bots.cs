using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case
{
    public class Bots
    {
        private Game botGame = new Game();
        public void SetBalance(int m)
        {
            botGame.balance = m;
        }

        private int bid;
        private int bidOn;
        private int balanceInf;
        public void DalamberBot(int amountOfBets, int minBid)
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

        public void MartingaleBot(int amountOfBets, int minBid)
        {
            bidOn = 0;
            bid = minBid;
            balanceInf = botGame.balance;
            int wins = 0;
            for (int i = 0; i < amountOfBets; i++)
            {
                botGame.ParityBet(bidOn, bid);
                if (balanceInf + bid == botGame.balance)
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
                balanceInf = botGame.balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total profit " + botGame.profit);
            Console.WriteLine("Amount of bets " + botGame.amountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }
    }
}
