using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public class Pad
	{
		public int[] Cards { get; private set; } // Last three cards are Jack, Queen and King; their values are 10; the order doesn't matter; ace is signed as 11

		public void Update()
		{
			int i = 2;
			while (i < 15)
			{
				if (Cards[i] <= 1) // Too few cards left
				{
					break;
				}
				if (i == 14)
				{
					return;
				}
				i++;
			}
			
			for (i = 2; i < 15; i++) // Recreating pad with all cards
			{
				Cards[i] = 32;
			}
		}

		public Pad()
		{
			Cards = new int[15] { 0, 0, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32, 32 }; // Creating pad for the first time
		}
	}
}