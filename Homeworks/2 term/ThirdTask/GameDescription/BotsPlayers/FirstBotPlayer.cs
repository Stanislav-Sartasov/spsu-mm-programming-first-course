using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class FirstBotPlayer : Player
	{
		public override void MakeBet()
		{
			Bet = (int)(0.05 * Cash);
			Cash -= Bet;
		}

		public override void Action(Pad pad)
		{
			if (true)
			{

			}
			//Hit
			//Stand
			//Double (может быть после split)
			//Split (новый конструктор, только 1 ход)
			//Surrender(только 1 ход)
		}
	}
}
