using System;
using System.Collections.Generic;
using RSQSortLib;

namespace SecondTask
{
	class Program
	{
		static void Main(string[] args)
		{
			var arr = new List<int>();

			RSQSort.Sort(arr.ToArray());
		}
	}
}
