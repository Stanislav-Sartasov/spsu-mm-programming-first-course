using System;
using WRMyHashTableClass;

namespace WRMyHashtable
{
	class Program
	{
		static void Main()
		{
			MyHashtable<int, string> table = new MyHashtable<int, string>(1000);

			for (int i = 0; i < 100; i++)
			{
				table.MyAdd(i, $"{i}");
			}
			table.Print();
			Console.WriteLine("\n\n");

			for (int i = 0; i < 50; i++)
			{
				table.MyDelete(i);
			}
			table.Print();
			Console.WriteLine("\n\n");

			string value;
			table.MySearch(2, out value);
			Console.WriteLine(value);
			table.Clear();

			table.MyAdd(5, "5");
			table.MyAdd(5, "5");
			table.MyAdd(6, "6");
			table.MyAdd(111, "111");
			table.MyAdd(20, "20");
			table.MySearch(5, out string s);
			Console.WriteLine(s);
			table.MyDelete(5);
			table.Print();
			table.Clear();
		}
	}
}
