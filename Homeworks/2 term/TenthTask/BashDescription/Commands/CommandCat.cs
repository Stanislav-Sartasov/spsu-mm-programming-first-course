using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	class CommandCat : Command
	{
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
		public override string RunCommand(string name, string str)
		{
			var resStr = "";
			var path = str.Substring(str.IndexOf(name) + name.Length + 1);
			path.Replace("\n", "");

			if (System.IO.File.Exists(path))
			{
				var file = new StreamReader(path.Replace("\n", ""));
				while (!file.EndOfStream)
				{

					var line = file.ReadLine();
					resStr += line;
					resStr += "\n";
					Console.WriteLine(line);
				}
			}
			else
			{
				Console.WriteLine("Error! No such file.");
			}

			return resStr;
		}
	}
}
