using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Hand
    {
        public List<Card> Cards { get; private set; }
        public int Points { get; private set; }
        public int CurrentBet { get; set; }
        public bool Boosted { get; private set; }
        public bool Surrender { get; set; }
        public int Aces { get; private set; }
        public int AcesAs11 { get; private set; }

        


        public Hand()
        {
            Cards = new List<Card>();
            Points = 0;
            Boosted = false;
            Surrender = false;
            Aces = 0;
            AcesAs11 = 0;
        }

        public void Reset()
        {
            Cards.Clear();
            Points = 0;
            Boosted = false;
            Surrender = false;
            Aces = 0;
            AcesAs11 = 0;
        }

        public void TakeCard(Card newCard)
        {
            if (!Boosted)
            {
                Cards.Add(newCard);
                if ((int)newCard.Value == 11)
                {
                    Aces++;
                    AcesAs11++;
                }

                Points += (int)newCard.Value;
                if (AcesAs11 != 0 && Points > 21)
                {
                    Points -= 10;
                    AcesAs11--;
                }
                CheckBoost();
            }
            else
                Console.WriteLine("Boosted!");
        }

        public Card PopCard()
        {
            var popCard = Cards[Cards.Count - 1];
            Cards.Remove(popCard);
            if ((int)popCard.Value == 11)
            {
                if (AcesAs11 < Aces) //проверяем, есть ли тузы со значением 1
                    Points -= 1;
                //else
                //иная ситуация не реализуется
                Aces--;
                AcesAs11--;
            }
            else
                Points -= (int)popCard.Value;
            return popCard;
        }

        public void PrintCards()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].PrintCard();
            }
        }

        public void CheckBoost()
        {
            if (Points > 21)
                Boosted = true;
        }

        public void PrintPoints()
        {
            Console.WriteLine(Points);
        }

        public void PrintBet()
        {
            Console.WriteLine(CurrentBet);
        }
    }
}
