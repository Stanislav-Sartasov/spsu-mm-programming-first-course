using System;
using System.Collections.Generic;
using System.Text;

namespace Casino_Case.Logic
{
    abstract public class Bet
    {

        public static int balance;
        public static int profit = 0;
        public static int AmountOfBets = 0;

        protected internal void InputChecking(int TBid)
        {
            if (TBid > Bet.balance)
                throw new Exception("You dont have enough money");
            if (TBid <= 0)
                throw new Exception("Bid can't be less than 1");
        }
        protected internal void OutputWin(int TBid, int TCoef)
        {
            Console.WriteLine("You've just won " + TBid * TCoef);
            Bet.balance += (TBid) * TCoef;
            Bet.profit += (TBid) * TCoef;
        }
        protected internal void OutputLose(int TBid)
        {
            Console.WriteLine("Didn't win :(");
            Bet.balance -= TBid;
            Bet.profit -= TBid;
        }

    }
}
