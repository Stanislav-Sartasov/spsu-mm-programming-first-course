using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette
{
	public class MartingaleBot:APlayer
	{
		public MartingaleBot()
		{
			name = "MartingaleBot";
			cash = 100000;
		}
		Game game = new Game();
		public override void Bet()
		{
			int lostMoney = 0;
			for (int i = 0; i < 400; i++)
			{
				if (lostMoney == 0)
					currentBet = 100;
				else
					currentBet = lostMoney + 100;
				if (cash >= currentBet)
				{
					bool result = game.CheckOfWin(Game.TypeOfBet.Even, 12);
					Winnings(game.GetCoefficient(Game.TypeOfBet.Even), result);
					if (result)
						lostMoney = 0;
					else
						lostMoney += currentBet;
				}
				else
				{
					while (cash < currentBet && currentBet > 100)
						currentBet /= 2;
				}	
				if (cash < 100)
				{
					Console.WriteLine("Martingale is bankrupt");
					break;
				}
			}
		}
	}
}
