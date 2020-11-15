using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	class Deck
	{
		private Card[] deck;
        private int count;
        private int index;
		public Deck()
		{
            count = 52 * 8;
            InitDeck();
		}
		public void InitDeck()
		{
            index = 0;
            Random rnd = new Random();
            int randFirst = 0, randSec = 0;
            int eight = 1;
            int color = 0;
            int val = 1;
            deck = new Card[count];
            for (int i = 0; i < count; ++i)
            {
                color++;
                if (color > 4)
                {
                    color = 1;
                    val++;
                    if (val > 13)
                    {
                        val = 2;
                        eight++;
                    }
                }
                deck[i] = new Card(eight, color, val);
            }
            for (int i = 0; i < count; ++i)
            {
                randFirst = rnd.Next(0, count);
                randSec = rnd.Next(0, count);
                var temp = deck[randFirst];
                deck[randFirst] = deck[randSec];
                deck[randSec] = temp;
            }
            Draw tabel = new Draw();
            tabel.InitDeck();
        }
        public Card GetCard()
        {
            if (index + 1 == count)
            {
                InitDeck();
            }
            else
                index++;
            return deck[index];
        }
	}
}
