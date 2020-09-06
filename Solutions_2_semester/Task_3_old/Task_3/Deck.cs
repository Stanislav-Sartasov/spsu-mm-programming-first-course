using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    class Deck
    {
        public Deck(int count)
        {
            cards = new Card[count * 52];
            for (int i = 0; i < count; i++)
                for (int s = 0; s < 4; s++)
                    for (int d = 1; d <= 13; d++)
                        cards[i * 52 + s * 13 + d - 1] = new Card() { Suit = Card.suitList[s], Dignity = d };
            activeCount = count * 52;
        }

        Card[] cards;
        int activeCount;
        Random rand = new Random();

        public Card TakeRand()
        {
            if (activeCount == 0)
                return null;
            int val = rand.Next(0, activeCount);
            Card res = cards[val];
            cards[val] = cards[--activeCount];
            return res;
        }
    }
}
