using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW6Bash
{
	class CatCommand
	{
		public static void Execute(string[] s, string[] buffer)
		{
			if (s.Length < 2)
				Console.WriteLine("Error 1");
			else
			{
				string fName = s[1];
				if (File.Exists(s[1]))
				{
					string[] allLines = File.ReadAllLines(fName);
					for (int i = 0; i < allLines.Length; i++)
					{
						Console.WriteLine(allLines[i]);
						buffer[0] += allLines[i];
					}
				}
				else
					Console.WriteLine("File not Found");
			}
		}
	}
}