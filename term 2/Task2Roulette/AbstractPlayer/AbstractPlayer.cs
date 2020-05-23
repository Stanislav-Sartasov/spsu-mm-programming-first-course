using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractPlayer
{
    public abstract class AbstractPlayer
    {
       
        protected string name;
        protected int wallet;
        protected int currentBet;
        protected int choice;
        protected int cell;

        public int GetBalance()
        {
            return wallet;
        }

        public int GetBet()
        {
            return currentBet;
        }
        public int GetChoice()
        {
            return choice;
        }

        public int GetCell()
        {
            return cell;
        }
        public virtual void RoundResult(bool result, int coefficient)
        {
            if (result)
                wallet += coefficient * currentBet;
            else
                wallet -= currentBet;
        }
        public abstract void Bet();

    }
}
