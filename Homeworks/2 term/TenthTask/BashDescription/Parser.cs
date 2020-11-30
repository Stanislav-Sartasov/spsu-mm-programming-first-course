using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	public class Parser
	{
		//0 echo
		//1 exit
		//2 pwd
		//3 cat
		//4 wc

		private static readonly string[] commands = { "echo", "exit", "pwd", "cat", "wc" };
		internal static Values Values { get; set; }

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
					return echo.CheckCommand(commands[commandNum], str);
				case 1:
					var exit = new CommandExit();
					return exit.CheckCommand(commands[commandNum], str);

				case 2:
					var pwd = new CommandPwd();
					return pwd.CheckCommand(commands[commandNum], str);
				case 3:
					var cat = new CommandCat();
					return cat.CheckCommand(commands[commandNum], str);

				case 4:
					var wc = new CommandWc();
					return wc.CheckCommand(commands[commandNum], str);
			}

			return true;
		}
		public Parser()
		{
			Values = new Values();
		}

		public string RunCommand(int commandNum, string str)
		{
			var resStr = "";
			switch (commandNum)
			{
				case 0:
					var echo = new CommandEcho();
					echo.RunCommand(commands[commandNum], str);
					break;
				case 1:
					var exit = new CommandExit();
					exit.RunCommand(commands[commandNum], str);
					break;

				case 2:
					var pwd = new CommandPwd();
					pwd.RunCommand(commands[commandNum], str);
					break;
				case 3:
					var cat = new CommandCat();
					cat.RunCommand(commands[commandNum], str);
					break;

				case 4:
					var wc = new CommandWc();
					wc.RunCommand(commands[commandNum], str);
					break;
			}

			return resStr;
		}

		public void Parse(string str)
		{
			var commandFlag = false;

			if (!str.Contains("|"))
			{
				for (int i = 0; i < commands.Length; i++)
				{
					if (str.Contains(commands[i]))
					{
						int pos = str.IndexOf(commands[i]);
						int commandNum = i;

						if (pos >= 0)
						{
							if (CheckCommand(commandNum, str))
							{
								RunCommand(commandNum, str);

							}
							else
							{
								Console.WriteLine("No such command");
							}

							commandFlag = true;
							break;

						}
					}
				}

				if (!commandFlag)
				{
					if (str.Contains("$"))
					{
						AddValueMethod(str);
					}
					else
					{
						Console.WriteLine("Wrong syntax.");
					}
				}
			}
			else
			{
				string[] partsOfStr = str.Split('|');
				string resStr = "";

				foreach (string part in partsOfStr)
				{
					for (int i = 0; i < commands.Length; i++)
					{
						if (part.Contains(commands[i]))
						{
							int pos = part.IndexOf(commands[i]);
							int commandNum = i;
							if (pos >= 0)
							{
								var checkStr = String.Concat(part, " ", resStr);

								if (CheckCommand(commandNum, checkStr))
								{
									resStr = RunCommand(commandNum, checkStr);

								}
								else
								{
									Console.WriteLine("No such command.");
								}

								break;
							}

							break;
						}
						else if (str.Contains("$"))
						{
							AddValueMethod(str);
						}
					}
				}
			}
		}
	}
}
