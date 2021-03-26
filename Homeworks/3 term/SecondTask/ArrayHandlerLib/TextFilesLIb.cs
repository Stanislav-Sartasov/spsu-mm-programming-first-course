using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ArrayHandlerLib
{
	public static class TextFilesLib
	{
		public static List<int> ReadArray(string path)
		{
			try
			{
				return File.ReadAllText(path).Split(new char[] { ' ' }).Select(x => int.Parse(x)).ToList();
			}
			catch
			{
				throw new Exception("Error in reading file.");
			}
		}

		public static void WriteArray(string path, List<int> arr)
		{
			try
			{
				File.WriteAllText(path, string.Join(" ", arr.Select(x => x.ToString())));
			}
			catch
			{
				throw new Exception("Error in writing file.");
			}
		}
	}
}
