using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public abstract class Person // Abstract player.
	{
		public int Cash { get; set; }
		public int FirstCard { get; set; }
		public int SecondCard { get; set; }
		public int OtherCards { get; set; }
		protected int NumOfAces { get; set; } // Aces in not main cards; need in sum-function
		protected virtual string InputForAction { get; set; }

		public int StandFlag {get; set;}

		protected void GetCard(Pad pad)
		{
			var random = new Random();
			int playerCard = random.Next(2, 11);
			while (pad.cards[playerCard] == 0) // Empty card in pad
			{
				playerCard = random.Next(2, 11);
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

		private int sumOfAllCards; // Using for surrender
		public int SumOfAllCards
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


		public void FirstCardsInitialization(Pad pad)
		{
			for (int i = 0; i < 2; i++)
			{
				GetCard(pad);
			}
		}

		public virtual void Action(Pad pad)
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

		public virtual void Clear()
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