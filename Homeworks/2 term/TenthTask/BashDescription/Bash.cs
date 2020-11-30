using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	public class Bash
	{
		internal static readonly string[] commands = { "echo", "exit", "pwd", "cat", "wc" };

		public Parser Parser { get; set; } = new Parser();

		internal Values Values { get; set; } = new Values();

		public void AddValueMethod(string str)
		{
			if (str.IndexOf("$") < 0)
			{
				return;
			}

			var val = str.Split(',');
			foreach (string resStr in val)
			{
				int pos = resStr.IndexOf("=");
				if (pos < 0)
				{
					if (Values.ValuesUsed.Contains(str.Substring(resStr.IndexOf("$") + 1).Replace(" ", "")))
					{
						Values.ValuesUsed.Clear();
						Values.ValuesMean.Clear();
						Console.WriteLine("Error! Redefinition.");
					}
					else
					{
						Values.ValuesUsed.Add(resStr.Substring(resStr.IndexOf("$")).Replace(" ", ""));
						Values.ValuesMean.Add("");
					}
				}
				else
				{
					if (!resStr.Substring(pos + 1).Contains("$"))
					{
						if (!Values.ValuesUsed.Contains(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
						    .Replace(" ", "")))
						{
							Values.ValuesUsed.Add(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
							    .Replace(" ", ""));

							Values.ValuesMean.Add(resStr.Substring(resStr.IndexOf("=") + 1));
						}
						else
						{
							Values.ValuesMean[Values.ValuesUsed.IndexOf(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
								.Replace(" ", ""))] =
							    resStr.Substring(resStr.IndexOf("=") + 1);
						}

					}
					else if (Values.ValuesUsed.Contains(resStr.Substring(pos + 1).Replace(" ", "")))
					{
						Values.ValuesMean[Values.ValuesUsed.IndexOf(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
						    .Replace(" ", ""))] = Values.ValuesMean[Values.ValuesUsed.IndexOf(resStr.Substring(pos + 1).Replace(" ", ""))];
					}
					else
					{
						Values.ValuesUsed.Clear();
						Values.ValuesMean.Clear();
						Console.WriteLine("Error! Values are used.");
					}
				}
			}
		}

		public bool CheckCommand(int commandNum, string str)
		{
			switch (commandNum)
			{
				case 0:
					var echo = new CommandEcho();
					return echo.CheckCommand(Bash.commands[commandNum], str);
				case 1:
					var exit = new CommandExit();
					return exit.CheckCommand(Bash.commands[commandNum], str);

				case 2:
					var pwd = new CommandPwd();
					return pwd.CheckCommand(Bash.commands[commandNum], str);
				case 3:
					var cat = new CommandCat();
					return cat.CheckCommand(Bash.commands[commandNum], str);

				case 4:
					var wc = new CommandWc();
					return wc.CheckCommand(Bash.commands[commandNum], str);
			}

			return true;
		}
		public string RunCommand(int commandNum, string str)
		{
			var resStr = "";
			switch (commandNum)
			{
				case 0:
					var echo = new CommandEcho();
					echo.RunCommand(Bash.commands[commandNum], str, this);
					break;
				case 1:
					var exit = new CommandExit();
					exit.RunCommand(Bash.commands[commandNum], str);
					break;

				case 2:
					var pwd = new CommandPwd();
					pwd.RunCommand(Bash.commands[commandNum], str);
					break;
				case 3:
					var cat = new CommandCat();
					cat.RunCommand(Bash.commands[commandNum], str);
					break;

				case 4:
					var wc = new CommandWc();
					wc.RunCommand(Bash.commands[commandNum], str);
					break;
			}

			return resStr;
		}

		public void Start()
		{
			while (true)
			{
				var str = Console.ReadLine();
				Parser.Parse(str, this);
			}
		}
	}
}

//на русском пишу для себя
//дописать системный вызов, разделить команды и парсер на классы
//покрасивее оформить строки (через автосвойства)
//абстрактный класс?