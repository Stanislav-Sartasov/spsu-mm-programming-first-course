using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    abstract public class Bet
    {
        protected void InputChecking(int TBid)
        {
            if (TBid > BetInfo.Balance)
                throw new Exception("You dont have enough money");
            if (TBid <= 0)
                throw new Exception("Bid can't be less than 1");
        }
        protected void OutputWin(int TBid, int TCoef)
        {
            Console.WriteLine("You've just won " + TBid * TCoef);
            BetInfo.Balance += (TBid) * TCoef;
            BetInfo.Profit += (TBid) * TCoef;
            BetInfo.AmountOfBets++;
        }
        protected void OutputLose(int TBid)
        {
            Console.WriteLine("Didn't win :(");
            BetInfo.Balance -= TBid;
            BetInfo.Profit -= TBid;
            BetInfo.AmountOfBets++;

            
        }

    }
}
