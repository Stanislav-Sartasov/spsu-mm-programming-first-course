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
				var text = File.ReadAllText(path).Split(' ');

				var temp = new List<int>();
				foreach (var t in text)
				{
					temp.Add(int.Parse(t));
				}

				return temp;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
				//throw new Exception("Error in reading file.");
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
