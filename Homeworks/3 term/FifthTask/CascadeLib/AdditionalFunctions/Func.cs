using System;

namespace CascadeLib
{
	public static class Func
	{
		public static int Square(int a) => (a * a);
		public static double Log(int length) => (Math.Log(length, 2));
		public static int NumOfBlocks(int length)
		{
			double log = Log(length);
			if (Math.Abs((log - Math.Floor(log))).CompareTo(0) > 0)
			{
				return (length / (int)log) + 1;
			}
			else
			{
				return length / (int)log;
			}
		}
	}
}