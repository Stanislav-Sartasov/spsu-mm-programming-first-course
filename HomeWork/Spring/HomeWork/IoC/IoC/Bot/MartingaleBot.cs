﻿using Casino_Case.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Bot
{
    
    public class MartingaleBot
    {
        private ParityBet BotParityBet = new ParityBet();
        public void Action(int AmountOfBets, int MinBid)
        {
            int BidOn = 0;
            int bid = MinBid;
            int BalanceInf = BetInfo.balance;
            int wins = 0;
            for (int i = 0; i < AmountOfBets; i++)
            {
                BotParityBet.Betting(BidOn, bid);
                if (BalanceInf + bid == BetInfo.balance)
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
                BalanceInf = BetInfo.balance;
            }

            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("Total profit " + BetInfo.profit);
            Console.WriteLine("Amount of bets " + BetInfo.AmountOfBets);
            Console.WriteLine("Amount of wins " + wins);
        }
    }
}
