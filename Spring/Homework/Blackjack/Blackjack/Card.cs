using System;

namespace Blackjack
{
    public class Card
    {
        public CardSuit Suit {get;}

        public CardValue Value { get;}

        public Card(CardSuit cardSuit, CardValue cardValue)
        {
            Suit = cardSuit;
            Value = cardValue;
        }

        public void PrintCard()
        {
            Console.WriteLine($"{Suit}:\t{Value}");
        }
    }
}
