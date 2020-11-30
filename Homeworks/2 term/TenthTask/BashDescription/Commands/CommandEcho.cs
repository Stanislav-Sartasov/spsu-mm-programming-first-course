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
					string[] val = Str.Substring(Str.IndexOf(name) + name.Length + 1).Split(' ');
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
			string[] vars = str.Substring(str.IndexOf(name) + name.Length).Split(' ');

			foreach (string varStr in vars)
			{

				if (varStr == "")
				{
					continue;
				}
				if (Parser.Values.ValuesUsed.Contains(varStr.Replace(" ", "")))
				{
					resStr = Parser.Values.ValuesMean[Parser.Values.ValuesUsed.IndexOf(varStr.Replace(" ", ""))].Replace("\"", "") + " ";
					Console.WriteLine(Parser.Values.ValuesMean[Parser.Values.ValuesUsed.IndexOf(varStr.Replace(" ", ""))].Replace("\"", "") + " ");
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
}
