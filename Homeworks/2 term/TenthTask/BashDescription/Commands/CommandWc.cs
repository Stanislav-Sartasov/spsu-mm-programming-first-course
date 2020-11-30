using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	class CommandWc : Command
	{
		//public string Str { get; set; }

		public override bool CheckCommand(string name, string str)
		{
			Str = str;
			if (Str.Substring(0, Str.IndexOf(name)).Replace(" ", "") != "")
			{
				return false;
			}
			else
			{
				try
				{
					var val = Str.Substring(Str.IndexOf(name) + name.Length + 1).Split(' ');
				}
				catch
				{
					return false;
				}

				return true;
			}
		}
		public override string RunCommand(string name, string str, Bash forValues = null)
		{
			var resStr = "";
			var path = str.Substring(str.IndexOf(name) + name.Length + 1);
			path.Replace("\n", "");

			if (System.IO.File.Exists(path))
			{
				resStr = String.Concat(File.ReadAllBytes(path).Length, " bytes ", File.ReadAllLines(path).Length, " strings");
				Console.WriteLine(String.Concat(File.ReadAllBytes(path).Length, " bytes"));
				Console.WriteLine(String.Concat(File.ReadAllLines(path).Length, " strings"));
			}
			else
			{
				Console.WriteLine("Error! No such file.");
			}
			return resStr;
		}
	}
}
