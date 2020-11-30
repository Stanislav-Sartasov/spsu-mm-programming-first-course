using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	class CommandPwd : Command
	{
		//public string Str { get; set; }

		public override bool CheckCommand(string name, string str)
		{
			Str = str;
			if (Str.Replace(" ", "") == "pwd")
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public override string RunCommand(string name, string str, Bash forValues = null)
		{
			var resStr = "";
			if (str.Replace(" ", "") == "pwd")
			{

				Console.WriteLine(Directory.GetCurrentDirectory());
				foreach (string dirStr in Directory.EnumerateFiles(Directory.GetCurrentDirectory()))
				{
					resStr = dirStr;
					Console.WriteLine("\t" + dirStr);
				}
			}
			else
			{
				Console.WriteLine("Error syntax");
			}

			return resStr;
		}
	}
}
