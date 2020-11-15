using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	class Interaction : IInteraction
	{
		private readonly IDraw drawer;

		public Interaction(IDraw start)
		{
			drawer = start;
		}
		public int GetInt()
		{
			int target;
			if (Int32.TryParse(Console.ReadLine(), out target))
			{
				return target;
			}
			else
			{
				drawer.IntError();

			}
			return 0;
		}

		public bool Ready()
		{
			int target;
			if (Int32.TryParse(Console.ReadLine(), out target))
			{
				if (target == 1)
					return true;
				else
					return false;
			}
			drawer.IntError();
			return false;
		}
		public SomeBet DoBet(SomeBet betNow, int moneyMoment)
		{
			int target = GetInt();
			if (target != 0)
				if (target != 1)
					if (target != 2)
						target = 0;
			betNow.target = target;
			int money = GetInt();
			if (money > moneyMoment)
				money = moneyMoment;
			betNow.money = money;
			return betNow;
		}
	}
}
