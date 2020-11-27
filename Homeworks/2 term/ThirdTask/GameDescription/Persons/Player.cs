using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	public abstract class Player : Person
	{
		protected Player SecondHand { get; set; }
		protected int SurrenderIsAllowed { get; set; }
		protected int DoubleIsAllowed { get; set; }
		public int Bet { get; set; }
		public abstract void MakeBet();

		public override void Action(Pad pad) 
		{
			if (SumOfAllCards() == 21)
			{
				ChangeStatus("blackjack");
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

							ChangeStatus("surrender");
						}

						SurrenderIsAllowed = 0;
					}
					else if (DoubleIsAllowed != 0 && InputForAction == "Double")
					{
						Cash -= Bet;
						Bet *= 2;
						Console.WriteLine();

						GetCard(pad);

						ChangeStatus("stand");

						DoubleIsAllowed = 0;
						SurrenderIsAllowed = 0;
					}

					if (PersonStatus != "surrender")
					{
						ChangeStatus();
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

		public abstract string IsContinue();
	}
}