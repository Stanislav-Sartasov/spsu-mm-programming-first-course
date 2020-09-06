using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class Card
    {
        public static readonly char[] suitList = new char[] { 'c', 'b', 'k', 'p' };
        char suitIn;
        int dignityIn;
        int costIn;
        public char Suit
        {
            get
            {
                return suitIn;
            }
            internal set
            {
                suitIn = value;
            }
        }
        public int Dignity
        {
            get
            {
                return dignityIn;
            }
            internal set
            {
                if (value >= 10)
                    costIn = 0;
                else
                    costIn = value;
                dignityIn = value;
            }
        }
        public int Cost
        {
            get
            {
                return costIn;
            }
        }
        public Card Clone()
        {
            Card newCard = new Card();
            newCard.suitIn = suitIn;
            newCard.dignityIn = dignityIn;
            newCard.costIn = costIn;
            return newCard;
        }
    }
}
