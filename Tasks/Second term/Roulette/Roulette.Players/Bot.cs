using Roulette.Bet;

using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Players
{
    public abstract class Bot : IPlayer
    {
        protected static Random brain = new Random();
        protected IBet myBet;
        protected int currentCashBet;
        protected int amountMoney;
        protected string Name;

        public abstract void MakeBet(int maxBet);

        public string ShowName()
        {
            return Name;
        }
        public int ShowBet()
        {
            return currentCashBet;
        }

        public int ShowMoney()
        {
            return amountMoney;
        }

        public virtual void MoneyRecount(int multiplier, int cashTable)
        {
            if (multiplier == 0)
                amountMoney -= currentCashBet;
            else
                amountMoney += Math.Min(cashTable, currentCashBet * multiplier);
        }

        public IBet ShowField()
        {
            return myBet;
        }

        public string ShowStatus()
        {
            string temp = myBet.Print();

            string result = $"Information about {Name}\n" +
                            $"Amount of money a player has = {amountMoney}. " +
                            $"{temp} with the amount of money {currentCashBet}\n";

            return result;
        }
    }
}
