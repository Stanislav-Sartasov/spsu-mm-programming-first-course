using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GameDescription
{
	public class FirstBotPlayer : Player
	{
		internal override void MakeBet()
		{
			Bet = (int)(0.05 * Cash);
			Cash -= Bet;
		}

		internal override void Action(Pad pad)
		{
			#region Stupid bot strategy.

			if (SumOfAllCards < 20)
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

		internal override bool IsContinue(int gamesLeft)
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