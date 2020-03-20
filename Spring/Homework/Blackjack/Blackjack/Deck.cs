using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public class Deck
    {
        public List<Card> Cards { get; private set; }
        public int NumOfDecks { get; private set; }

        public Deck(int number)
        {
            Cards = new List<Card>();
            NumOfDecks = number;
            for (int i = 0; i < NumOfDecks; i++)
            {
                foreach (var cardSuit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
                {
                    foreach (var cardValue in Enum.GetValues(typeof(CardValue)).Cast<CardValue>())
                        Cards.Add(new Card(cardSuit, cardValue));
                }
            }
            Shuffle();
        }

        public Deck()
            : this(8)
        {
        }

        public void Reset()
        {
            Cards.Clear();
            Cards = new List<Card>();

            for (int i = 0; i < NumOfDecks; i++) //default value of decks is 8
            {
                foreach (var cardSuit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
                {
                    foreach (var cardValue in Enum.GetValues(typeof(CardValue)).Cast<CardValue>())
                        Cards.Add(new Card(cardSuit, cardValue));
                }
            }
            Shuffle();
        }

        private void Shuffle()
        {
            int randomIndex;
            var rand = new Random();
            for (int i = 0; i < Cards.Count; i++)
            {
                randomIndex = rand.Next(0, Cards.Count - 1);

                Card oldCard = Cards[i];
                Cards[i] = Cards[randomIndex];
                Cards[randomIndex] = oldCard;
            }
        }

        public Card GetCard()
        {
            int lastIndex = Cards.Count - 1;
            Card popCard = Cards[lastIndex];
            Cards.Remove(Cards[lastIndex]);
            return popCard;
        }
    }
}
