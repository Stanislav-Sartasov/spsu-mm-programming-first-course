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

		public override string RunCommand(string str, Values values = null)
		{
			try
			{
				string name = "pwd";
				Str = str;
				if (Str.Replace(" ", "") == name)
				{
					var resStr = "";
					if (str.Replace(" ", "") == name)
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
				else
				{
					throw new Exception();
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
