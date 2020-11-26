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
			#region Stupid bot strategy.

			//apply strategy для каждой из колод - впихнуть split
			if (SumOfAllCards() < 20)
			{
				if (DoubleIsAllowed == 1)
				{
					InputForAction = "Double";
				}
				else
				{
					InputForAction = "Hit";
				}
			}
			else
			{
				InputForAction = "Stand";
			}

			base.Action(pad);

			#endregion
		}
	}
}