using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	class Human : Player
	{
		public override void ChangeStatus()
		{
			if (FirstCard + SecondCard == 21) // блэкджек
			{
				GameStatus = 1;
			}
			else if (SumOfAllCards() > 21) // выбыл
			{
				GameStatus = 0;
			}
			else // в игре
			{
				GameStatus = 2;
			}
			
		}
		public void ChangeStatusExit(int isInGame)
		{
			if (isInGame == 0) // не в игре
			{
				GameStatus = 3;
			}
		}
	}
}