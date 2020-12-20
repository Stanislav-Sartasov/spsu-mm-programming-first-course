using System;
using System.Collections.Generic;
using System.Text;
using BashDescription.Commands;

namespace BashDescription
{
	public class Parser
	{
		private byte CommandFlag { get; set; } = 0;

		public List<Command> Parse(string input)
		{
			var list = new List<Command>();

			foreach (string command in input.Trim().Split('|'))
			{
				VariableCheck(command.Trim());

				if (CommandFlag == 0)
				{
					list.Add(GetCommand(command.Trim()));
				}
			}

			CommandFlag = 0;
			return list;
		}

		private Command GetCommand(string input)
		{
			var temp = input.Split(' ');
			var args = string.Join(' ', temp, 1, temp.Length - 1);

			if (temp[0] == "echo")
			{
				var echoArg = "";

				foreach (string oldArg in args.Split(' '))
				{
					if (oldArg[0] == '$' && Bash.Variables.TryGetValue(oldArg.Remove(0, 1), out string value))
					{
						echoArg = String.Concat(echoArg, value, " ");
					}
					else
					{
						echoArg = String.Concat(echoArg, oldArg, " ");
					}
				}

				args = echoArg;
			}

			var output = NewCommand(temp[0], args);
			if (output == null)
			{
				output = new Command(input);
				
			}

			return output;
		}
		private Command NewCommand(string choise, string args)
		{
			Command command = choise switch
			{
				"echo" => new CommandEcho(args),
				"exit" => new CommandExit(args),
				"pwd" => new CommandPwd(args),
				"cat" => new CommandCat(args),
				"wc" => new CommandWc(args),
				_ => null,
			};
			return command;
		}

		private void VariableCheck(string command)
		{
			if (command[0].Equals('$') && command.Contains("="))
			{
				var temp = command.Remove(0, 1).Split('=');

				if (temp.Length == 2)
				{
					temp[0] = temp[0].Trim();
					temp[1] = temp[1].Trim();

					if (temp[1][0].Equals('$'))
					{
						temp[1] = temp[1].Remove(0, 1);

						if (Bash.Variables.TryGetValue(temp[1], out string value))
						{
							if (!Bash.Variables.TryAdd(temp[0], value))
							{
								Bash.Variables[temp[0]] = value;
							}

							CommandFlag = 1;
						}
						else
						{
							throw new Exception("Undefined valuabale.");
						}
					}
					else
					{
						if (!Bash.Variables.TryAdd(temp[0], temp[1]))
						{
							Bash.Variables[temp[0]] = temp[1];
						}

						CommandFlag = 1;
					}
				}
				else
				{
					throw new Exception("Incorrect input.");
				}
			}
		}
	}
}