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
		//public string Str { get; set; }
		public override string RunCommand(string str, Values values = null)
		{
			try
			{
				string name = "cat";
				Str = str;

				if (Str.Substring(0, Str.IndexOf(name)).Replace(" ", "") != "")
				{
					throw new Exception();
				}
				else
				{
					try
					{
						var val = Str.Substring(Str.IndexOf(name) + name.Length + 1).Split(' ');
					}
					catch
					{
						throw new Exception();
					}

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
						throw new Exception("Error! No such file.");
					}

					return resStr;
				}
			}
			catch(Exception e)
			{
				throw e;
			}
		}
	}
}
