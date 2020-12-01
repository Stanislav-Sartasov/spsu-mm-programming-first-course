using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public class SecondBotPlayer : Player
	{
		public override void MakeBet()
		{
			Bet = 25;
			Cash -= Bet;
		}

		public override void Action(Pad pad)
		{
			#region Less stupid bot strategy.

			if (SumOfAllCards < 8 || (SumOfAllCards < 17 && SumOfAllCards > 11))
			{
				InputForAction = "Hit";
			}
			else if (SumOfAllCards < 12 && SumOfAllCards > 8)
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

		public override bool IsContinue(int gamesLeft)
		{
			if (gamesLeft > 0)
			{
				if (Cash <= 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}
	}
}