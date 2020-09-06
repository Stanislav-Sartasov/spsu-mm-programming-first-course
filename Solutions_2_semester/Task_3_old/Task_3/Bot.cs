using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class Bot : Player
    {
        double leaveBudgetUpCoeff = 5;
        double leaveBudgetDownCoeff = 0.01;
        bool autoLeavingIn = false;
        protected int startBudget = -1;

        public double LeaveBudgetUpCoeffecient
        {
            get
            {
                return leaveBudgetUpCoeff;
            }
            set
            {
                if (place == null && value >= 1)
                    leaveBudgetUpCoeff = value;
            }
        }
        public double LeaveBudgetDownCoefficient
        {
            get
            {
                return leaveBudgetDownCoeff;
            }
            set
            {
                if (place == null && value >= 0 && value <= 1)
                    leaveBudgetDownCoeff = value;
            }
        }
        public bool AutoLeaving
        {
            get
            {
                return autoLeavingIn;
            }
            set
            {
                if (place == null)
                    autoLeavingIn = value;
            }
        }
        public void ResetStartBudget()
        {
            startBudget = Budget;
        }
        protected bool NeedToLeave()
        {
            return (autoLeavingIn && (Budget > startBudget * leaveBudgetUpCoeff || Budget < startBudget * leaveBudgetDownCoeff)) || Budget <= 0;
        }
    }
}
