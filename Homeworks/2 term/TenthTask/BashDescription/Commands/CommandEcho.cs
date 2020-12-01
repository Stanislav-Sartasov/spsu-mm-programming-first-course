using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	class CommandEcho : Command
	{
		//public string Str { get; set; }

		public override string RunCommand(string str, Values values)
		{
			try
			{
				string name = "echo";
				Str = str;
				if (Str.Substring(0, Str.IndexOf(name)).Replace(" ", "") != "")
				{
					throw new Exception();
				}
				else
				{
					try
					{
						string[] val = Str.Substring(Str.IndexOf(name) + name.Length + 1).Split(' ');
					}
					catch
					{
						throw new Exception();
					}


					var resStr = "";
					string[] vars = str.Substring(str.IndexOf(name) + name.Length).Split(' ');

					foreach (string varStr in vars)
					{

						if (varStr == "")
						{
							continue;
						}
						if (values.ValuesUsed.Contains(varStr.Replace(" ", "")))
						{
							resStr = values.ValuesMean[values.ValuesUsed.IndexOf(varStr.Replace(" ", ""))].Replace("\"", "") + " ";
							Console.WriteLine(values.ValuesMean[values.ValuesUsed.IndexOf(varStr.Replace(" ", ""))].Replace("\"", "") + " ");
						}
						else
						{
							resStr += varStr;
							Console.WriteLine(varStr + " ");
						}

					}
					return resStr;

				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}
	}
}
