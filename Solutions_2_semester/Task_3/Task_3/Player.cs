using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class Player
    {
        string nameIn = "????";
        public string Name
        {
            get
            {
                return nameIn;
            }
            set
            {
                if (place == null)
                    nameIn = value;
            }
        }
        protected int budgetIn = 0;
        protected bool areBudgetedIn = false;
        protected Random rand = new Random();
        internal GameMaster.PlayerPlace place = null;
        public bool AreBudgeted
        {
            get
            {
                return areBudgetedIn;
            }
            internal set
            {
                if (!areBudgetedIn)
                    areBudgetedIn = value;
            }
        }
        public int Budget
        {
            get
            {
                return budgetIn;
            }
            internal set 
            {
                if ((long)value > 0xfffffff)
                    budgetIn = 0xfffffff;
                else
                    budgetIn = value;
                AreBudgeted = true;
            }
        }
        public void SetRand(int value)
        {
            rand = new Random(value);
        }
        public void SetRand(Random rand)
        {
            this.rand = rand;
        }
        public virtual int MakeBate(int count, int type)
        {
            if (count > 0 && type >= 0 && type < 3 && place != null)
            {
                if (place.AreBated)
                    return -1;
                if (count >= Budget)
                    count = Budget;
                place.MakeBate(count, type);
                return 0;
            }
            else
                return -1;
        }
        public virtual int MakeBate()
        {
            if (place != null)
            {
                if (place.AreBated)
                    return -1;
                if (Budget > 2000)
                    MakeBate(rand.Next(1, Budget / 2), rand.Next(0, 3));
                else
                    MakeBate(rand.Next(1, Budget), rand.Next(0, 3));
                return 0;
            }
            else
                return -1;
        }
        public virtual int LeaveActiveGameSession()
        {
            if (place != null)
            {
                place.Leave();
                return 0;
            }
            else
                return -1;
        }
    }
}
