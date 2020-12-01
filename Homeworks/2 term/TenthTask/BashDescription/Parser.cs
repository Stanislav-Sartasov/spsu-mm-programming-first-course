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

		public void Parse(string str, Values values)
		{
			var commandFlag = false;

			if (!str.Contains("|"))
			{
				for (int i = 0; i < Bash.commands.Length; i++)
				{
					if (str.Contains(Bash.commands[i]))
					{
						int pos = str.IndexOf(Bash.commands[i]);
						int commandNum = i;

						if (pos >= 0)
						{
							try
							{
								Bash.RunCommand(commandNum, str);

							}
							catch
							{
								Console.WriteLine("Error, probably no such command.");
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
						AddValueMethod(str, values);
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
				foreach (string part in partsOfStr)
				{
					for (int i = 0; i < Bash.commands.Length; i++)
					{
						if (part.Contains(Bash.commands[i]))
						{
							int pos = part.IndexOf(Bash.commands[i]);
							int commandNum = i;
							if (pos >= 0)
							{
								string resStr = "";
								var checkStr = String.Concat(part, " ", resStr);

								try
								{
									resStr = Bash.RunCommand(commandNum, checkStr);

								}
								catch
								{
									Console.WriteLine("Error, probably no such command.");
								}

								break;
							}

							break;
						}
						else if (str.Contains("$"))
						{
							AddValueMethod(str, values);
						}
					}
				}
			}
		}

		public void AddValueMethod(string str, Values values)
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
					if (values.ValuesUsed.Contains(str.Substring(resStr.IndexOf("$") + 1).Replace(" ", "")))
					{
						values.ValuesUsed.Clear();
						values.ValuesMean.Clear();
						Console.WriteLine("Error! Redefinition.");
					}
					else
					{
						values.ValuesUsed.Add(resStr.Substring(resStr.IndexOf("$")).Replace(" ", ""));
						values.ValuesMean.Add("");
					}
				}
				else
				{
					if (!resStr.Substring(pos + 1).Contains("$"))
					{
						if (!values.ValuesUsed.Contains(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
						    .Replace(" ", "")))
						{
							values.ValuesUsed.Add(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
							    .Replace(" ", ""));

							values.ValuesMean.Add(resStr.Substring(resStr.IndexOf("=") + 1));
						}
						else
						{
							values.ValuesMean[values.ValuesUsed.IndexOf(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
								.Replace(" ", ""))] =
							    resStr.Substring(resStr.IndexOf("=") + 1);
						}

					}
					else if (values.ValuesUsed.Contains(resStr.Substring(pos + 1).Replace(" ", "")))
					{
						values.ValuesMean[values.ValuesUsed.IndexOf(resStr.Substring(resStr.IndexOf("$"), pos - resStr.IndexOf("$"))
						    .Replace(" ", ""))] = values.ValuesMean[values.ValuesUsed.IndexOf(resStr.Substring(pos + 1).Replace(" ", ""))];
					}
					else
					{
						values.ValuesUsed.Clear();
						values.ValuesMean.Clear();
						Console.WriteLine("Error! Values are used.");
					}
				}
			}
		}
	}
}