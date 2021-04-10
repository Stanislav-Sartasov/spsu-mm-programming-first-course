using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GraphGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			StreamWriter streamWriter = new StreamWriter("out.txt");
			Random rand = new Random();
			int size;

			for (;;)
				try
				{
					size = int.Parse(Console.ReadLine());
					if (size <= 2)
						throw new Exception("size must be greater than 2");
					break;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					Console.WriteLine("try again");
				}

			streamWriter.WriteLine(size);

			Hashtable table = new Hashtable();
			for (int i = 0; i < rand.Next(size, size * size / 2); i++)
			{
				for (; ; )
				{
					int a = rand.Next(0, size - 1);
					int b = rand.Next(a + 1, size);
					if (table[a] == null)
						table[a] = new HashSet<int>();

					if (((HashSet<int>)table[a]).Contains(b))
						continue;

					streamWriter.WriteLine($"{a} {b} {rand.Next(0, 10000)}");
					((HashSet<int>)table[a]).Add(b);
					break;
				}
			}

			streamWriter.Close();
		}
	}
}
