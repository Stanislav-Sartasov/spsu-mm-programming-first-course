using System;
using System.Collections.Generic;
using System.Text;

namespace GameDescription
{
	public class Pad
	{
		public int[] cards = new int[12] { 0, 0, 32, 32, 32, 32, 32, 32, 32, 32, 128, 32 }; // It is assumed that at a nominal value of 10 the number of cards of each dignity (10, king, queen, jack) is less than 32
	}
}