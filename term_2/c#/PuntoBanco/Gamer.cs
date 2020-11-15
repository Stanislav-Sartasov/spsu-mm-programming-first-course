using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	abstract public class Gamer
	{
		protected int startMoney;
		public int moneyMoment;
		public virtual SomeBet MakeBet()
		{
			Random rnd = new Random();
			SomeBet betNow;
			betNow.money = rnd.Next(moneyMoment);
			moneyMoment -= betNow.money;
			betNow.man = -1;
			betNow.target = rnd.Next(4);
			return betNow;
		}
		public virtual void Recive(int money)
		{
			moneyMoment += money;
		}
	}
}
