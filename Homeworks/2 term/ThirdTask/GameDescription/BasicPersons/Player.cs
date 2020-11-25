﻿using System;
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
			if (InputForAction == "Hit" || InputForAction == "Stand")
			{
				base.Action(pad);

				SplitAndSurrenderIsAllowed = 0;
				DoubleIsAllowed = 0; // взял - не можешь сдаться
			}
			else
			{
				if (SplitAndSurrenderIsAllowed != 0 && InputForAction == "Split")
				{
					if (InputForAction == "Surrender")
					{
						int sum = (int)(0.5 * Bet);
						Cash += sum;
						Bet -= sum;

						ChangeStatus(0);
					}
					if (InputForAction == "Split")
					{
						//разделение колоды новым конструктором
						SecondHandHandler("create");

						ChangeStatus(2);
						SecondHand.ChangeStatus(2);
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

					ChangeStatus(3); // больше игрок не ходит

					// деление только первым ходом, дальше - нельзя
					DoubleIsAllowed = 0;
					SplitAndSurrenderIsAllowed = 0;
				}

				if (GameStatus != 0)
				{
					ChangeStatus();
				}
			}
		}
		public override void ChangeStatus(int prm = -1)
		{
			if (prm != -1)
			{
				base.ChangeStatus(prm); // Stand
			}
			else
			{
				if (FirstCard + SecondCard == 21)
				{
					GameStatus = 1; // Blackjack
				}
				else if (SumOfAllCards() > 21)
				{
					GameStatus = 0; // Lose
				}
				else if (GameStatus != 3)
				{
					GameStatus = 2; // In game
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