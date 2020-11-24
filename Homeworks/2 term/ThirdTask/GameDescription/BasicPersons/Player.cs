using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	//словарь??
	public abstract class Player : Person
	{
		protected int SplitIsAllowed { get; set; }
		protected int DoubleIsAllowed { get; set; }
		protected int SurrenderIsAllowed { get; set; }
		public int Bet { get; set; }
		public abstract void MakeBet();

		public override void Action(Pad pad) 
		{
			if (InputForAction == "Hit" || InputForAction == "Stand")
			{
				base.Action(pad);
			}
			else
			{
				if (SplitIsAllowed != 0 && InputForAction == "Split")
				{
					//разделение колоды новым конструктором
					SplitIsAllowed = 0;
				}
				else if (DoubleIsAllowed != 0 && InputForAction == "Double")
				{
					Cash -= Bet;
					Bet *= 2;
					DoubleIsAllowed = 0;
					//добавление третьей карты в сумму, удввоение ставки
				}
				else if (SurrenderIsAllowed != 0 && InputForAction == "Surrender")
				{
					int sum = (int)(0.5 * Bet);
					Cash += sum;
					Bet -= sum;
					ChangeStatus(0);
					SurrenderIsAllowed = 0;
					//выход из игры
				}
				if (GameStatus != 0)
				{
					ChangeStatus(-1);
				}
			}
		}
		public override void ChangeStatus(int prm) // проверка суммы, переписать без условий
		{
			if (prm != -1)
			{
				base.ChangeStatus(prm); // Stand
			}
			else if (FirstCard + SecondCard == 21)
			{
				GameStatus = 1; // Blackjack
			}
			else if (SumOfAllCards() > 21)
			{
				GameStatus = 0; // Lose
			}
			else
			{
				GameStatus = 2; // In game
			}

		}

		public override void Clear()
		{
			base.Clear();
			Bet = 0;
			SplitIsAllowed = 1;
			DoubleIsAllowed = 1;
			SurrenderIsAllowed = 1;
		}

		public Player() : base()
		{
			Cash = 500;
			Bet = 0;

			SplitIsAllowed = 1;
			DoubleIsAllowed = 1;
			SurrenderIsAllowed = 1;
		}
	}
}