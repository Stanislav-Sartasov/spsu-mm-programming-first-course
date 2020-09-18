using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
    public class Person : Gamer
    {
        private readonly IInteraction inter;
        public Person(IInteraction f)
        {
            inter = f;
            Draw user = new Draw();
            user.AskMoneyToGo();
            startMoney = inter.GetInt();
            moneyMoment = startMoney;
        }
        public Person()
        {
            Draw user = new Draw();
            Interaction inter = new Interaction(user);
            user.AskMoneyToGo();
            startMoney = inter.GetInt();
            moneyMoment = startMoney;
        }
        public override SomeBet MakeBet()
        {
            Draw user = new Draw();
            user.DoBet(moneyMoment);
            SomeBet betNow;
            betNow.man = 0;
            betNow.target = 0;
            betNow.money = 0;
            betNow = inter.DoBet(betNow, moneyMoment);
            betNow.man = 0;
            moneyMoment -= betNow.money;
            return betNow;
        }
    }
}
