using Casino_Case.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Bot
{

    public class MartingaleBot : Bots
    {
        public void Action(int AmountOfBets, int MinBid)
        {
            int BidOn = 0;
            int bid = MinBid;
            int BalanceInf = Bet.balance;
            int wins = 0;
            for (int i = 0; i < AmountOfBets; i++)
            {
                BotParityBet.Betting(BidOn, bid);
                if (BalanceInf + bid == Bet.balance)
                {

                    if (BidOn == 0)
                        BidOn = 1;
                    else
                        BidOn = 0;
                    bid = MinBid;
                    wins++;
                }
                else
                {
                    bid *= 2;
                }
                BalanceInf = Bet.balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total profit " + Bet.profit);
            Console.WriteLine("Amount of bets " + Bet.AmountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }
    }
}
