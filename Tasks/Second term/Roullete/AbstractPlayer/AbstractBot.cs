using System;
using System.Collections.Generic;
using System.Text;

namespace Players
{
    public abstract class AbstractBot : AbstractPlayer
    {
        protected int PreviousCashBet;
        protected byte LogicLevel;
        protected static Random Brain = new Random();
        protected static Random Humanfactor = new Random();
    }
}
