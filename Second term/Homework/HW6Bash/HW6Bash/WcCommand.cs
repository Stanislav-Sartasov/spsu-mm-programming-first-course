using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW6Bash
{
	class WcCommand
	{
		public static void Execute(string[] s)
		{
			if (s.Length < 2)
				Console.WriteLine("Error 1");
			else
			{
				string fName = s[1];
				if (File.Exists(s[1]))
				{
					long fSize = new FileInfo(fName).Length;
					Console.WriteLine("Size of File = " + fSize + " bytes");

					using (StreamReader sr = File.OpenText(fName))
					{
						int lineCount = 1;
						int wordCount = 0;
						bool lastCharLetter = false;
						for (int i = 0; ; i++)
						{
							int k = sr.Read();
							if (k >= 65 && k <= 90 || k >= 97 && k <= 122)
								lastCharLetter = true;
							else
							{
								if (lastCharLetter)
									wordCount++;

								lastCharLetter = false;

								if (k == '\n')
									lineCount++;
								if (k < 0)
									break;
							}
						}
						Console.WriteLine("Total words = " + wordCount);
						Console.WriteLine("Total lines = " + lineCount);
					}
				}
				else
					Console.WriteLine("File not Found");
			}
		}
	}
}
