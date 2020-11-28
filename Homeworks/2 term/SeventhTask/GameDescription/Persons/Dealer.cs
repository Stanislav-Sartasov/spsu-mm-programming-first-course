using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public class Dealer : Person
	{
		public override void Action(Pad pad)
		{
			#region Dealer strategy: while he can - he hits.

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