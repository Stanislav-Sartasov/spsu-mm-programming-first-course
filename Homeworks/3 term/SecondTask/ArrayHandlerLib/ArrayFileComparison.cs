using System;
using System.IO;
using System.Linq;

namespace ArrayHandlerLib
{
	public static class ArrayFileComparison
	{
		public static bool CompareTwoFileArrays(string fileFirst, string fileSecond)
		{
			if (!File.Exists(fileFirst))
			{
				Console.WriteLine($"File {fileFirst} does not exist.");
				return false;
			}

			if (!File.Exists(fileSecond))
			{
				Console.WriteLine($"File {fileSecond} does not exist.");
				return false;
			}

			string contentsFirst = File.ReadAllText(fileFirst);
			string contentsSecond = File.ReadAllText(fileSecond);

			if (contentsFirst.Length != contentsSecond.Length)
			{
				Console.WriteLine("File sizes are different.");
				return false;
			}

			for (int i = 0; i < contentsFirst.Length; i++)
			{
				if (contentsFirst[i] != contentsSecond[i])
				{
					Console.WriteLine($"Files are different at position {i}");
					return false;
				}
			}

			Console.WriteLine("Files are the same.");
			return true;
		}
	}
}
