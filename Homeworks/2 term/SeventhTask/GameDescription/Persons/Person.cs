using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public abstract class Person // Abstract player.
	{
		public int Cash { get; set; }
		public string PersonStatus { get; set; } // Status of player/dealer
		public int FirstCard { get; set; }
		public int SecondCard { get; set; }
		public int OtherCards { get; set; }
		protected int NumOfAces { get; set; } // Aces in not main cards; need in sum-function

		private static int RandomCard()
		{
			var random = new Random();
			return random.Next(2, 11);
		}
		protected void GetCard(Pad pad)
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
				if (result + 11 <= 21) // Big Ace
				{
					result += 11;
				}
				else // Little Ace
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
				else if (i > 0) // Overshoot
				{
					result -= 9;
				}
				else // Ovetshoot without aces
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
			ChangeStatus();
		}

		protected virtual string InputForAction { get; set; }
		public virtual void Action(Pad pad)
		{
			switch (InputForAction)
			{
				case "Hit":
					GetCard(pad);
					break;
				case "Stand":
					ChangeStatus("stand");
					break;
			}
			ChangeStatus();
		}

		public void ChangeStatus(string sts = "no_status")
		{
			if (sts != "no_status")
			{
				PersonStatus = sts; // Stand (17-21 for dealer)/Surrender/BJ with OtherCards
			}
			else
			{
				if (FirstCard + SecondCard == 21)
				{
					PersonStatus = "blackjack"; // BlackJack
				}
				else if (SumOfAllCards() > 21)
				{
					PersonStatus = "lose"; // Lose (>21)
				}
				else if (PersonStatus != "stand")
				{
					PersonStatus = "in_game"; // In game (<17 for dealer)
				}
			}
		}

		public virtual void Clear()
		{
			PersonStatus = "in_game";
			FirstCard = 0;
			SecondCard = 0;
			OtherCards = 0;
			NumOfAces = 0;
		}
	}
}