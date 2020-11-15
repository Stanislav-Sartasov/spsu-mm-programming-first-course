using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoBanco
{
	public class Card
	{
		public readonly int deck;
		public readonly int color;
		public readonly int cost;
		public Card (int deck, int color, int cost)
		{
			this.deck = deck < 10 ? deck : 0;
			this.color = color < 10 ? color : 1;
			this.cost = cost < 14 ? cost : 1;
		}
		private string GetColor()
		{
			string str = "none";
			switch (color)
			{
				case 1:
					str = "Cherva";
					break;
				case 2:
					str = "Buba";
					break;
				case 3:
					str = "Krest";
					break;
				case 4:
					str = "Pika";
					break;

			}
			return str;
		}
		private string GetCost()
		{
			string str = "none";
			if (cost < 10)
			{
				if (cost == 1)
				{
					str = "A";
				}
				else
				{
					str = cost.ToString();
				}
			}
			else
			{
				switch (cost)
				{
					case 10:
						str = "10";
						break;
					case 11:
						str = "J";
						break;
					case 12:
						str = "Q";
						break;
					case 13:
						str = "K";
						break;
				}
			}
			return str;
		}
		public string GetCard()
		{
			return GetCost() + " " + GetColor();
		}
		public int OldStyleGet()
		{
			return 1000 * deck + 100 * color + cost;
		}
	}
}
