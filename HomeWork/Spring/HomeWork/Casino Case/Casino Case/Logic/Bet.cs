using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    abstract public class Bet
    {
        internal void InputChecking(int TBid)
        {
            if (TBid > BetInfo.balance)
                throw new Exception("You dont have enough money");
            if (TBid <= 0)
                throw new Exception("Bid can't be less than 1");
        }
        internal void OutputWin(int TBid, int TCoef)
        {
            Console.WriteLine("You've just won " + TBid * TCoef);
            BetInfo.balance += (TBid) * TCoef;
            BetInfo.profit += (TBid) * TCoef;
            BetInfo.AmountOfBets++;
        }
        internal void OutputLose(int TBid)
        {
            Console.WriteLine("Didn't win :(");
            BetInfo.balance -= TBid;
            BetInfo.profit -= TBid;
            BetInfo.AmountOfBets++;

            
        }

    }
}
