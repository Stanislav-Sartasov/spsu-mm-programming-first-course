using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	public class GamerFirst : Gamer
	{
		public GamerFirst(int money)
		{
			startMoney = money;
			moneyMoment = money;
		}
		public override SomeBet MakeBet()
		{
			SomeBet betNow;
			betNow.man = 1;
			betNow.target = 0;
			betNow.money = (int)(0.125 * moneyMoment);
			moneyMoment -= betNow.money;
			return betNow;
		}
	}
}
