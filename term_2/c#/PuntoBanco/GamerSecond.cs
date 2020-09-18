using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
    public class GamerSecond : Gamer
    {
        public GamerSecond(int money)
        {
            startMoney = money;
            moneyMoment = money;
        }
        public override SomeBet MakeBet()
        {
            SomeBet betNow;
            betNow.man = 2;
            betNow.target = 1;
            betNow.money = (int)(0.25 * moneyMoment);
            moneyMoment -= betNow.money;
            return betNow;
        }
    }
}
