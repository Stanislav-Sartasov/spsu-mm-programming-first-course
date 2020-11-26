using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Dealer : Person
	{
		public override void Action(Pad pad)
		{
			#region Dealer strategy: while it can - hit.

			if (SumOfAllCards() >= 17)
			{
				InputForAction = "Stand";
			}
			else
			{
				InputForAction = "Hit";
			}
			base.Action(pad);

			#endregion
		}
		
		public Dealer() : base()
		{
			Cash = 10000;
		}
	}
}