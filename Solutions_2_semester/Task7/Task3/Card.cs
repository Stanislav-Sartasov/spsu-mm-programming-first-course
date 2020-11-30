
namespace Task3
{
	public class Card
	{
		public Card(int value, Suits suit)
		{
			if (value >= MinValue && value <= MaxValue)
				Value = value;
			else
				Value = 0;

			Suit = suit;

			if (value < MinValue || value >= 10)
				Cost = 0;
			else
				Cost = value;
		}
		public enum Suits
		{
			Hearts,
			Diamonds,
			Clubs,
			Spades
		}
		public const int SuitsCount = 4;
		public const int MinValue = 1;
		public const int MaxValue = 13;

		public int Value { get; private set; }
		public Suits Suit { get; private set; }
		public int Cost { get; private set; }
	}
}