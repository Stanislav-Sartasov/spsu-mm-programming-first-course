using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArrayHandlerLib
{
	public static class ArrayGeneration
	{
		public static void GenerateTwoArrays(string dirPath)
		{
			Random r = new(DateTime.Now.Millisecond);
			int capacity = 100; //10000000

			List<int> lst = new(capacity);
			for (int i = 0; i < capacity; i++)
			{
				lst.Add(r.Next(1000001));
			}

			File.WriteAllText(dirPath + "unsorted.dat", string.Join(" ", lst.Select(x => x.ToString())));

			lst.Sort();
			File.WriteAllText(dirPath + "sorted.dat", string.Join(" ", lst.Select(x => x.ToString())));
		}
	}
}
