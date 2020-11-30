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

		public void Parse(string str, Bash forValues)
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
							if (forValues.CheckCommand(commandNum, str))
							{
								forValues.RunCommand(commandNum, str);

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
						forValues.AddValueMethod(str);
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
					for (int i = 0; i < Bash.commands.Length; i++)
					{
						if (part.Contains(Bash.commands[i]))
						{
							int pos = part.IndexOf(Bash.commands[i]);
							int commandNum = i;
							if (pos >= 0)
							{
								var checkStr = String.Concat(part, " ", resStr);

								if (forValues.CheckCommand(commandNum, checkStr))
								{
									resStr = forValues.RunCommand(commandNum, checkStr);

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
							forValues.AddValueMethod(str);
						}
					}
				}
			}
		}
	}
}
