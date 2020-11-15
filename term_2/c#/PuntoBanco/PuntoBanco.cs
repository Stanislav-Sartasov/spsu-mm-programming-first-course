using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	class PuntoBanco
	{
		public Card[] cards;
		private int score;
		public PuntoBanco()
		{
			cards = new Card[3];
			for (int i = 0; i < 3; ++i)
				cards[i] = null;
		}
		public void GetCard(Card first, Card second)
		{
			cards[0] = first;
			cards[1] = second;
			int firstScore = first.cost < 10 ? first.cost : 0;
			int secScore = second.cost < 10 ? second.cost : 0;
			score = (firstScore + secScore) % 10;
		}
		public void GetCard(Card first)
		{
			cards[2] = first;
			int firstScore = first.cost < 10 ? first.cost : 0;
			score = (firstScore + score) % 10;
		}
		public bool IsNatural()
		{
			if (cards[2] == null)
				return score > 7 ? true : false;
			return false;
		}
		public bool CanTake()
		{
			if (cards[2] == null)
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
