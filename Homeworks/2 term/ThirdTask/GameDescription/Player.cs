using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	//словарь??
	public class Player : Person
	{
		public int Bet { get; set; }
		public virtual void MakeBet() { }

		public override void Action(Pad pad) // номер хода??
		{

		}
		public override void ChangeStatus() // проверка суммы
		{
			if (FirstCard + SecondCard == 21) // блэкджек
			{
				GameStatus = 1;
			}
			else if (SumOfAllCards() > 21) // выбыл
			{
				GameStatus = 0;
			}
			else // в игре
			{
				GameStatus = 2;
			}

		}

		public override void ApplyTurn(string turn, Pad pad)
		{
			base.ApplyTurn(turn, pad);
			switch (turn)
			{
				case "Split":

					break;
				case "Double":

					break;
				case "Surrender":

					break;
			}
		}

		public override void Clear()
		{
			base.Clear();
			Bet = 0;
		}

		public Player()
		{
			Cash = 500;
			Bet = 0;
			GameStatus = 0;
			FirstCard = 0;
			SecondCard = 0;
			OtherCards = 0;
			NumOfAces = 0;
		}
	}
}