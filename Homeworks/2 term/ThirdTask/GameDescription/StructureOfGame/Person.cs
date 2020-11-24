using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public abstract class Person // Abstract player.
	{
		public int Cash { get; set; }
		public int GameStatus { get; set; } // Status of player
		public int FirstCard { get; set; }
		public int SecondCard { get; set; }
		public int OtherCards { get; set; }
		public int BlackJack { get; set; } // BJ flag
		public int NumOfAces { get; set; } // Aces; need in sum-function

		private static int RandomCard()
		{
			var random = new Random();
			return random.Next(2, 11);
		}
		public void GetCard(Pad pad)
		{
			int playerCard = RandomCard();
			while (pad.cards[playerCard] == 0) // Empty card in pad
			{
				playerCard = RandomCard();
			}

			if (FirstCard == 0)
			{
				FirstCard = playerCard;
			}
			else if (SecondCard == 0)
			{
				SecondCard = playerCard;
			}
			else
			{
				if (playerCard == 11)
				{
					NumOfAces++;
				}
				else
				{
					OtherCards += playerCard;
				}
			}
			pad.cards[playerCard]--;
		}
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
				if (result + 11 <= 21) // большой туз
				{
					result += 11;
				}
				else // малый туз
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

		public void FirstCardsInitialization(Pad pad)
		{
			for (int i = 0; i < 2; i++)
			{
				GetCard(pad);
			}
			ChangeStatus(-1);
		}

		protected virtual string InputForAction { get; set; }
		public virtual void Action(Pad pad)
		{
			switch (InputForAction)
			{
				case "Hit":
					GetCard(pad);
					ChangeStatus(2);
					break;
				case "Stand":
					ChangeStatus(3);
					break;
				default:
					return;
			}
			ChangeStatus(-1);
		}
		public virtual void ChangeStatus(int prm) // PersonStatus // заменить int флаги на строки
		{
			if (prm != -1)
			{
				GameStatus = prm;
			}
		}

		public virtual void Clear()
		{
			GameStatus = 0;
			FirstCard = 0;
			SecondCard = 0;
			OtherCards = 0;
			NumOfAces = 0;
			BlackJack = 0;
		}
	}
}
