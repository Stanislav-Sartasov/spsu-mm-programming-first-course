using System;

namespace Roulette.People
{
    public abstract class AbstractPlayer
    {
        protected string playerName;
        protected char[] bet = new char[2];
        protected int sumBet;
        protected int amountMoney;

        public char[] ViewBet()
        {
            return bet;
        }

        public int ViewAmountOfMoney()
        {
            return amountMoney;
        }

        public int ViewSumBet()
        {
            return sumBet;
        }

        public virtual void MoneyRecount(bool roundResult, int multiplier, int maxSum)
        {
            if (roundResult)
                amountMoney += Math.Min(maxSum, (multiplier) * sumBet);
            else
                amountMoney -= sumBet;
        }

        public abstract void SetBet(int maxBet);

        public string ShowStatus()
        {
            string result = $"Information about {playerName}\n" +
                            $"Amount of money a player has = {amountMoney}. " +
                            $"Last bet is {bet[0]}{bet[1]} with the amount of money {sumBet}\n";

            return result;
        }
    }
}
