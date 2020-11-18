using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW6Bash
{
	class PwdCommand
	{
		public static void Execute()
		{
			string curDir = Directory.GetCurrentDirectory();
			Console.WriteLine("Current directory is " + curDir);
			string[] listOfFiles = Directory.GetFiles(curDir);
			for (int i = 0; i < listOfFiles.Length; i++)
			{
				Console.WriteLine(listOfFiles[i]);
			}
		}
	}
}
