using System;

namespace CascadeLib
{
	public static class ArrayGenerator
	{
		public static int[] Generate(int capacity, int maxValue)
		{
			if (capacity < 0)
			{
				Console.WriteLine("Capacity should be non-negative number!");
				return null;
			}

			int[] temp;
			if (capacity == 0)
			{
				temp = new int[1];
				temp[0] = 0;
			}
			else
			{
				temp = new int[capacity];
				var rnd = new Random();

				for (int i = 0; i < capacity; i++)
				{
					temp[i] = rnd.Next(maxValue + 1);
				}
			}
			return temp;
		}
	}
}