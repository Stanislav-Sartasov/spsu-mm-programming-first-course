using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	class CommandExit : Command
	{
		//public string Str { get; set; }

		public override bool CheckCommand(string name, string str)
		{
			Str = str;
			{
				if (Str.Substring(0, Str.IndexOf(name)).Replace(" ", "") != "")
				{
					return false;
				}
				else
				{
					try
					{
						int checker = Convert.ToInt32(Str.Substring(Str.IndexOf(name) + name.Length + 1));
						return true;
					}
					catch
					{
						return false;
					}
				}
			}
		}
		public override string RunCommand(string name, string str)
		{
			var resStr = "";
			var vars = str.Substring(str.IndexOf(name) + name.Length + 1);

			if (int.TryParse(vars, out int y))
			{
				Environment.Exit(0);
			}
			else
			{
				Console.WriteLine("Error.");
			}

			return resStr;
		}
	}
}
