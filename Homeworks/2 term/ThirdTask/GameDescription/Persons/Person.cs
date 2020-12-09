using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public abstract class Person // Abstract player.
	{
		public int Cash { get; internal set; } // Using in unit tests
		internal int FirstCard { get; set; }
		internal int SecondCard { get; set; }
		internal int OtherCards { get; set; }
		private int NumOfAces { get; set; } // Aces in not main cards; need in sum-function
		protected string InputForAction { get; set; }
		internal int StandFlag {get; set;}

		private int sumOfAllCards; // Using for surrender
		internal int SumOfAllCards
		{
			get
			{
				if (sumOfAllCards == 50)
				{
					return sumOfAllCards;
				}
				else
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
			}
		set
			{
				sumOfAllCards = value;
			}
		}

		protected void GetCard(Pad pad)
		{
			var random = new Random();
			int playerCard = random.Next(2, 14);
			while (pad.Cards[playerCard] == 0) // Empty card in pad
			{
				playerCard = random.Next(2, 14);
			}

			if (FirstCard == 0)
			{
				if (playerCard > 11) // J/Q/K
				{
					FirstCard = 10;
				}
				else
				{
					FirstCard = playerCard;
				}
			}
			else if (SecondCard == 0)
			{
				if (playerCard > 11)
				{
					SecondCard = 10;
				}
				else
				{
					SecondCard = playerCard;
				}
			}
			else
			{
				if (playerCard > 11)
				{
					OtherCards += 10;
				}
				else if (playerCard == 11)
				{
					NumOfAces++;
				}
				else
				{
					OtherCards += playerCard;
				}
			}

			pad.Cards[playerCard]--;
			pad.Update(); // Checking if too few cards left; can happen during any card distribution 
		}
		internal void FirstCardsInitialization(Pad pad)
		{
			for (int i = 0; i < 2; i++)
			{
				GetCard(pad);
			}
		}

		internal virtual void Action(Pad pad)
		{
			switch (InputForAction)
			{
				case "Hit":
					GetCard(pad);
					break;
				case "Stand":
					StandFlag = 1;
					break;
			}
		}

		internal virtual void Clear()
		{
			FirstCard = 0;
			SecondCard = 0;
			OtherCards = 0;
			NumOfAces = 0;
			StandFlag = 0;
			sumOfAllCards = 0;
		}
	}
}