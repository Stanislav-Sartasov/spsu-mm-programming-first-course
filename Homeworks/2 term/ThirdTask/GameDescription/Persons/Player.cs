using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GameDescription
{
	public abstract class Player : Person
	{
		protected Player SecondHand { get; set; }
		protected int SurrenderIsAllowed { get; set; }
		protected int DoubleIsAllowed { get; set; }
		public int Bet { get; set; }
		
		public abstract void MakeBet();
		public abstract bool IsContinue(int gamesLeft);

		public override void Action(Pad pad) 
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

		public override void Clear()
		{
			base.Clear();
			Bet = 0;

			SurrenderIsAllowed = 1;
			DoubleIsAllowed = 1;
		}

		public Player() : base()
		{
			Cash = 500;
			Bet = 0;

			SecondHand = null;

			SurrenderIsAllowed = 1;
			DoubleIsAllowed = 1;
		}

	}
}