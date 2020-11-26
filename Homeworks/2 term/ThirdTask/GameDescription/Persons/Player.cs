using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	public abstract class Player : Person
	{
		protected Player SecondHand { get; set; }
		protected int SplitAndSurrenderIsAllowed { get; set; } // сдаться и разделить можно только на 1 ходу
		protected int DoubleIsAllowed { get; set; } // удвоил - не можешь сдаться
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

					SplitAndSurrenderIsAllowed = 0;
					DoubleIsAllowed = 0; // взял - не можешь сдаться
				}
				else
				{
					if (SplitAndSurrenderIsAllowed != 0 && (InputForAction == "Split" || InputForAction == "Surrender"))
					{
						if (InputForAction == "Surrender")
						{
							int sum = (int)(0.5 * Bet);
							Cash += sum;
							Bet -= sum;

							ChangeStatus("surrender");
						}
						if (InputForAction == "Split")
						{
							//разделение колоды новым конструктором
							SecondHandHandler("create");

							ChangeStatus("in_game");
							SecondHand.ChangeStatus("in_game");
							// колоду для вывода надо обработать как вторую;  в идеале - черз массив (структур) в Action
						}

						SplitAndSurrenderIsAllowed = 0;
					}
					else if (DoubleIsAllowed != 0 && InputForAction == "Double")
					{
						Cash -= Bet;
						Bet *= 2;
						Console.WriteLine();

						GetCard(pad);

						ChangeStatus("stand"); // больше игрок не ходит

						// деление только первым ходом, дальше - нельзя
						DoubleIsAllowed = 0;
						SplitAndSurrenderIsAllowed = 0;
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

			SplitAndSurrenderIsAllowed = 1;
			DoubleIsAllowed = 1;
		}

		public Player() : base()
		{
			Cash = 500;
			Bet = 0;

			SecondHand = null;

			SplitAndSurrenderIsAllowed = 1;
			DoubleIsAllowed = 1;
		}

		protected void SecondHandHandler(string prm) // обработчик второй руки
		{
			switch (prm)
			{
				case "create":

					break;
				case "destroy":

					break;
			}
		}
	}
}

//Hit
//Stand
//Double (может быть после split)
//Split (новый конструктор, только 1 ход)
//Surrender(только 1 ход)