using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class SecondBotStrategy : Player
	{
		public override void MakeBet()
		{
			Bet = 25;
			Cash -= Bet;
		}

		public override void Action(Pad pad)
		{
			#region Clever bot strategy.

			//apply strategy для каждой из колод - впихнуть split
			if (SumOfAllCards() < 8 || (SumOfAllCards() < 17 && SumOfAllCards() > 11))
			{
				InputForAction = "Hit";
			}
			else if (SumOfAllCards() < 12 && SumOfAllCards() > 8) 
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