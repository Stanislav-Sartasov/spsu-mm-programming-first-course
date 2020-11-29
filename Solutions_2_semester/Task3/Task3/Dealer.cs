
namespace Task3
{
	public class Dealer
	{
		public Dealer(Deck deck)
		{
			cardPull = new Card[6];

			for (int i = 0; i < 6; i++)
				if (i == 2 || i == 5)
					cardPull[i] = null;
				else
					cardPull[i] = deck.Next();

			PlayerScore = (cardPull[0].Cost + cardPull[1].Cost) % 10;
			BankScore = (cardPull[3].Cost + cardPull[4].Cost) % 10;
			PlayerScoreBeforeExtraCard = PlayerScore;
			BankScoreBeforeExtraCard = BankScore;

			if (PlayerScore < 8 && BankScore < 8)
				if (PlayerScore < 6)
				{
					cardPull[2] = deck.Next();
					PlayerScore = (PlayerScore + cardPull[2].Cost) % 10;
					if (BankScore <= 2)
						cardPull[5] = deck.Next();
					else if (BankScore == 3)
					{
						if (cardPull[2].Value != 8)
							cardPull[5] = deck.Next();
					}
					else if (BankScore == 4)
					{
						if (cardPull[2].Value >= 2 && cardPull[2].Value <= 7)
							cardPull[5] = deck.Next();
					}
					else if (BankScore == 5)
					{
						if (cardPull[2].Value >= 4 && cardPull[2].Value <= 7)
							cardPull[5] = deck.Next();
					}
					else if (BankScore == 6)
						if (cardPull[2].Value == 6 || cardPull[2].Value == 7)
							cardPull[5] = deck.Next();
				}
				else if (BankScore < 6)
					cardPull[5] = deck.Next();

			if (cardPull[5] != null)
				BankScore = (BankScore + cardPull[5].Cost) % 10;

			if (PlayerScore > BankScore)
				WinField = Field.Player;
			else if (BankScore > PlayerScore)
				WinField = Field.Bank;
			else
				WinField = Field.Draw;
		}

		Card[] cardPull;
		public Card[] CardPull
		{ 
			get
			{
				return (Card[])cardPull.Clone();
			}
		}
		public int PlayerScoreBeforeExtraCard { get; private set; }
		public int PlayerScore { get; private set; }
		public int BankScoreBeforeExtraCard { get; private set; }
		public int BankScore { get; private set; }
		public Field WinField { get; private set; }
	}
}