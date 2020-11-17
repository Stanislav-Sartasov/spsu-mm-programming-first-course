
namespace Task3
{
	public class Card
	{
		public Card(int value, Suits suit)
		{
			if (value >= MinValue && value <= MaxValue)
				this.value = value;
			else
				this.value = 0;

			this.suit = suit;

			if (value < MinValue || value >= 10)
				cost = 0;
			else
				cost = value;
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

		public readonly int value;
		public readonly Suits suit;
		public readonly int cost;
	}
}