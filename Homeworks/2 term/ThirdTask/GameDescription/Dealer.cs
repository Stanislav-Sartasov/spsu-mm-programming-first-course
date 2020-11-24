using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Dealer : Person
	{
		public override void ChangeStatus()
		{
			if (FirstCard + SecondCard == 21) // блэкджек (выиграл)
			{
				GameStatus = 1;
			}
			else if (SumOfAllCards() > 21) // >21 (выбыл, берёт остаток)
			{
				GameStatus = 0; 
			}
			else if (SumOfAllCards() < 17) // <17 (берёт)
			{
				GameStatus = 3;
			}
			else // от 17 до 21 (не берёт)
			{
				GameStatus = 2;
			}
		}
		public override void Action(Pad pad)
		{
			for (int i = 0; i < 2; i++)
			{
				GetCard(pad);
			}
			ChangeStatus();

			while (GameStatus == 3)
			{
				GetCard(pad);
				ChangeStatus();
			}
		}

		public Dealer()
		{
			Cash = 10000;
			GameStatus = 0;
			FirstCard = 0;
			SecondCard = 0;
			OtherCards = 0;
			NumOfAces = 0;
		}
	}
}
