using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GameDescription
{
	public abstract class Player : Person
	{
		protected int SurrenderIsAllowed { get; set; }
		protected int DoubleIsAllowed { get; set; }
		internal int Bet { get; set; }
		
		internal abstract void MakeBet();
		internal abstract bool IsContinue(int gamesLeft);

		internal override void Action(Pad pad) 
		{
			if (SumOfAllCards == 21)
			{
				return;
			}
			else
			{
				if (InputForAction == "Hit" || InputForAction == "Stand")
				{
					base.Action(pad);

					SurrenderIsAllowed = 0;
					DoubleIsAllowed = 0;
				}
				else
				{
					if (SurrenderIsAllowed != 0)
					{
						if (InputForAction == "Surrender")
						{
							int sum = (int)(0.5 * Bet);
							Cash += sum;
							Bet -= sum;

							SumOfAllCards = 50;
						}
						else if (DoubleIsAllowed != 0 && InputForAction == "Double")
						{
							Cash -= Bet;
							Bet *= 2;

							GetCard(pad);

							StandFlag = 2;

							DoubleIsAllowed = 0;
						}

						SurrenderIsAllowed = 0;
					}
					else if (DoubleIsAllowed != 0 && InputForAction == "Double")
					{
						Cash -= Bet;
						Bet *= 2;

						GetCard(pad);

						StandFlag = 1;

						DoubleIsAllowed = 0;
						SurrenderIsAllowed = 0;
					}
				}
			}
		}

		internal override void Clear()
		{
			base.Clear();
			Bet = 0;

			SurrenderIsAllowed = 1;
			DoubleIsAllowed = 1;
		}

		protected Player() : base()
		{
			Cash = 500;
			Bet = 0;

			SurrenderIsAllowed = 1;
			DoubleIsAllowed = 1;
		}

	}
}