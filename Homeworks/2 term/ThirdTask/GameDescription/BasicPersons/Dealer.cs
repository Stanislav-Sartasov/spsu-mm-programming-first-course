using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Dealer : Person
	{
		public override void Action(Pad pad) // Dealer strategy
		{
			while (SumOfAllCards() < 17)
			{
				if (SumOfAllCards() >= 17)
				{
					ChangeStatus(3);
					break;
				}

				InputForAction = "Hit";
				base.Action(pad);	
			}
		}
		public override void ChangeStatus(int prm)
		{
			if (prm != -1)
			{
				base.ChangeStatus(prm); // от 17 до 21 (не берёт) // Stand
			}
			else if (FirstCard + SecondCard == 21) // блэкджек (выиграл)
			{
				GameStatus = 1; // BlackJack
			}
			else if (SumOfAllCards() > 21) // >21 (выбыл, берёт остаток)
			{
				GameStatus = 0; // Lose
			}
			else // <17 (берёт)
			{
				GameStatus = 2; // In game
			}
		}
		
		public Dealer() : base()
		{
			Cash = 10000;
		}
	}
}
