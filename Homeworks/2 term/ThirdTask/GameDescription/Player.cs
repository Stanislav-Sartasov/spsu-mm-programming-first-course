using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ThirdTask.GameDescription
{
	//словарь??
	public abstract class Player
	{
		public int Cash { get; set; }
		public int Bet { get; set; }
		public int GameStatus { get; set; }
		public int FirstCard { get; set; }
		public int SecondCard { get; set; }
		public int OtherCards { get; set; }
		public int NumOfAces { get; set; }
		public int Turn()
		{
			var rnd = new Random();
			return rnd.Next(2, 11);
		}
		public abstract void ChangeStatus();
		public int SumOfAllCards()
		{
			int result = OtherCards;
			if (FirstCard != 11)
			{
				result += FirstCard;
			}
			if (SecondCard != 11)
			{
				result += SecondCard;
			}

			if (FirstCard == 11)
			{
				if (result + 11 <= 21)
				{
					result += 11;
				}
				else
				{
					result += 1;
				}
			}
			if (SecondCard == 11)
			{
				if (result + 11 <= 21)
				{
					result += 11;
				}
				else
				{
					result += 1;
				}
			}

			for (int i = 0; i < NumOfAces; i++)
			{
				if (result + 11 <= 21)
				{
					result += 11;
				}
				else if (result + 1 <= 21)
				{
					result += 1;
				}
				else if (i > 0) // туз при переборе
				{
					result -= 9;
				}
				else // перебор без туза
				{
					result += 1;
				}				
			}
			return result;
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