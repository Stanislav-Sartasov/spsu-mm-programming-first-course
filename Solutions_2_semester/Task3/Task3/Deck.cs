using System;

namespace Task3
{
    public class Deck
    {
        public Deck(int count)
        {
            if (count <= 0)
                count = 1;
            ActualLength = (Card.MaxValue - Card.MinValue + 1) * Card.SuitsCount * count;
            cards = new Card[ActualLength];
            int c = 0;
            for (int i = Card.MinValue; i <= Card.MaxValue; i++)
                for (Card.Suits j = 0; (int)j < Card.SuitsCount; j++)
                    for (int g = 0; g < count; g++)
                        cards[c++] = new Card(i, j);
        }

        Card[] cards;
        public int ActualLength { get; private set; }
        static Random random = new Random();
        public Card Next()
        {
            int pos = random.Next(ActualLength);
            Card outCard = cards[pos];
            cards[pos] = cards[--ActualLength];
            return outCard;
        }
    }
}
