using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	class Bash
	{
		//0 echo
		//1 exit
		//2 pwd
		//3 cat
		//4 wc

		private readonly string[] commands = { "echo", "exit", "pwd", "cat", "wc" };
		private List<string> ValuesUsed { get; set; } = new List<string>();
		private List<string> ValuesMean { get; set; } = new List<string>();

		public bool CheckCommand(int commandNum, string str)
		{
			switch (commandNum)
			{
				case 0:
					if (str.Substring(0, str.IndexOf(commands[commandNum])).Replace(" ", "") != "")
					{
						return false;
					}
					else
					{
						try
						{
							string[] val = str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length + 1).Split(' ');
						}
						catch
						{
							return false;
						}

						return true;
					}

				case 1:
					{
						if (str.Substring(0, str.IndexOf(commands[commandNum])).Replace(" ", "") != "")
						{
							return false;
						}
						else
						{
							try
							{
								if (commandNum < 0 || commandNum > 255)
								{
									throw new Exception();
								}
								else
								{
									int checker = Convert.ToInt32(str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length + 1));
									return true;
								}
							}
							catch
							{
								return false;
							}
						}
					}

				case 2:
					{
						if (str.Replace(" ", "") == commands[2])
						{
							return true;
						}
						else
						{
							return false;
						}
					}

				case 3:
					{
						if (str.Substring(0, str.IndexOf(commands[commandNum])).Replace(" ", "") != "")
						{
							return false;
						}
						else
						{
							try
							{
								var val = str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length + 1).Split(' ');
							}
							catch
							{
								return false;
							}

							return true;
						}
					}

				case 4:
					{
						if (str.Substring(0, str.IndexOf(commands[commandNum])).Replace(" ", "") != "")
						{
							return false;
						}
						else
						{
							try
							{
								var val = str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length + 1).Split(' ');
							}
							catch
							{
								return false;
							}

							return true;
						}
					}
			}

			return true;
		}

		public void AddValueUsed(string str)
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
					if (ValuesUsed.Contains(str.Substring(resStr.IndexOf("$") + 1).Replace(" ", "")))
					{
						ValuesUsed.Clear();
						ValuesMean.Clear();
						Console.WriteLine("Error! Redefinition.");
					}
					else
					{
						ValuesUsed.Add(resStr.Substring(resStr.IndexOf("$")).Replace(" ", ""));
						ValuesMean.Add("");
					}
				}
				else
				{
					if (!resStr.Substring(pos + 1).Contains("$"))
					{
						if (!ValuesUsed.Contains(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
						    .Replace(" ", "")))
						{
							ValuesUsed.Add(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
							    .Replace(" ", ""));

							ValuesMean.Add(resStr.Substring(resStr.IndexOf("=") + 1));
						}
						else
						{
							ValuesMean[ValuesUsed.IndexOf(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
								.Replace(" ", ""))] =
							    resStr.Substring(resStr.IndexOf("=") + 1);
						}

					}
					else if (ValuesUsed.Contains(resStr.Substring(pos + 1).Replace(" ", "")))
					{ 
						ValuesMean[ValuesUsed.IndexOf(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
						    .Replace(" ", ""))] = ValuesMean[ValuesUsed.IndexOf(resStr.Substring(pos + 1).Replace(" ", ""))];
					}
					else
					{
						ValuesUsed.Clear();
						ValuesMean.Clear();
						Console.WriteLine("Error! Values are used.");
					}
				}
			}
		}

		public string RunCommand(int commandNum, string str)
		{
			var resStr = "";
			switch (commandNum)
			{
				case 0:
					{
						string[] vars = str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length).Split(' ');

						foreach (string varStr in vars)
						{

							if (varStr == "")
							{
								continue;
							}
							if (ValuesUsed.Contains(varStr.Replace(" ", "")))
							{
								resStr = ValuesMean[ValuesUsed.IndexOf(varStr.Replace(" ", ""))].Replace("\"", "") + " ";
								Console.WriteLine(ValuesMean[ValuesUsed.IndexOf(varStr.Replace(" ", ""))].Replace("\"", "") + " ");
							}
							else
							{
								resStr += varStr;
								Console.WriteLine(varStr + " ");
							}

						}
						return resStr;
					}

				case 1:
					{
						var vars = str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length + 1);

						if (int.TryParse(vars, out int y))
						{
							System.Diagnostics.Process.GetCurrentProcess().Kill(); // закрытие приложения
						}
						else
						{
							Console.WriteLine("Error.");
						}

						return resStr;

					}

				case 2:
					{
						if (str.Replace(" ", "") == "pwd")
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
							Console.WriteLine("Error syntax.");
						}

						return resStr;

					}

				case 3:
					{
						var path = str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length + 1);
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

				case 4:
					{
						var path = str.Substring(str.IndexOf(commands[commandNum]) + commands[commandNum].Length + 1);
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
						AddValueUsed(str);
					}
					else
					{
						Console.WriteLine("Wrong syntax");
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
									Console.WriteLine("No such command");
								}

								break;
							}

							break;
						}
						else if (str.Contains("$"))
						{
							AddValueUsed(str);
						}
					}
				}
			}
		}
	}
}