using HTClass;
using System;

namespace HT
{
	class Program
	{
		static void Main()
		{
			MyHashtable<int, int> table = new MyHashtable<int, int>();

			for (int i = 0; i < 100; i++)
			{
				table.MyAdd(i, i);
			}
			table.Print();
			Console.WriteLine("\n\n");

			for (int i = 0; i < 50; i++)
			{
				table.MyDelete(i);
			}
			table.Print();
			Console.WriteLine("\n\n");

			int value;
			table.MySearch(2, out value);
			Console.WriteLine(value);
		}
	}
}
