using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Dealer : Person
	{
		public override void Action(Pad pad)
		{
			//Dealer strategy: while it can - hit.

			//Console.WriteLine(11);

			if (SumOfAllCards() >= 17) // если заменить на сумму - не работает
			{
				InputForAction = "Stand";
			}
			else
			{
				InputForAction = "Hit";
			}
			base.Action(pad);

			////
		}
		public override void ChangeStatus(int prm = -1)
		{
			if (prm != -1)
			{
				base.ChangeStatus(prm); // Stand (17-21)
			}
			else
			{
				if (FirstCard + SecondCard == 21)
				{
					GameStatus = 1; // BlackJack
				}
				else if (SumOfAllCards() > 21)
				{
					GameStatus = 0; // Lose (>21)
				}
				else if (GameStatus != 3)
				{
					GameStatus = 2; // In game (<17)
				}
			}
		}
		
		public Dealer() : base()
		{
			Cash = 10000;
		}
	}
}
