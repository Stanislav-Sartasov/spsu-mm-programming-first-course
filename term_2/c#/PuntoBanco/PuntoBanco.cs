using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// punto = 0; banco = 1; tie = 2;
// person = 0;

namespace PuntoBanco
{
    class PuntoBanco
    {
        public int[] cards;
        private int score;
        public PuntoBanco()
        {
            cards = new int[3];
            score = 0;
            for (int i = 0; i < cards.Length; ++i)
                cards[i] = 0;
        }
        public void getCard(int first, int second)
        {
            cards[0] = first;
            cards[1] = second;
            int firstScore = first % 100 < 10 ? first % 100 : 0;
            int secScore = second % 100 < 10 ? second % 100 : 0;
            score = (firstScore + secScore) % 10;
        }
        public void getCard(int first)
        {
            cards[2] = first;
            int firstScore = first % 100 < 10 ? first % 100 : 0;
            score = (firstScore + score) % 10;
        }
        public bool isNatural()
        {
            if (cards[2] == 0)
                return score > 7 ? true : false;
            return false;
        }
        public bool CanTake()
        {
            if (cards[2] == 0)
                return score < 6 ? true : false;
            return false;
        }

        public static bool operator >(PuntoBanco p, PuntoBanco b)
        {
            return p.score > b.score;
        }
        public static bool operator <(PuntoBanco p, PuntoBanco b)
        {
            return p.score < b.score;
        }

    }
}
