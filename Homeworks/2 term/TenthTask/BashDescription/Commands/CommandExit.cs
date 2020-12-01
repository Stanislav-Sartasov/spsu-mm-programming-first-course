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

		public override string RunCommand(string str, Values values = null)
		{
			try
			{
				string name = "exit";
				Str = str;
				{
					if (Str.Substring(0, Str.IndexOf(name)).Replace(" ", "") != "")
					{
						throw new Exception();
					}
					else
					{
						try
						{
							int checker = Convert.ToInt32(Str.Substring(Str.IndexOf(name) + name.Length + 1));

							var resStr = "";
							var vars = str.Substring(str.IndexOf(name) + name.Length + 1);

							if (int.TryParse(vars, out int y))
							{
								Environment.Exit(0);
							}
							else
							{
								throw new Exception("Error, the correct format is \"echo <int>\".");
							}

							return resStr;
						}
						catch
						{
							throw new Exception();
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
