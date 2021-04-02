using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette
{
	public class LightBot:APlayer
	{
		readonly Game game = new Game();
		public LightBot()
		{
			name = "LightBot";
			cash = 100000;
		}
		public override void Bet()
		{
			
			for (int i = 0; i < 400; i++)
			{
				currentBet = 1000;
				while (cash < currentBet && cash > 100)
				{
					currentBet /= 2;
				}
				if (currentBet < 100)
				{
					Console.WriteLine("LightBot is bankrupt");
					break;
				}
				else
				{
					bool result = game.CheckOfWin(Game.TypeOfBet.Black, 15);
					Winnings(game.GetCoefficient(Game.TypeOfBet.Black), result);
				}
			}
		}
	}
}
