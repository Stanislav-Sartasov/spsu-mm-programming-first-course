using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW6Bash
{
	class PhaseOneCommand
	{
		public static int Execute(int commandsLeft, string[] realCommand, string[] commands, int[] commandsCount)
		{
			if (commandsLeft > 0)
			{
				realCommand[0] = commands[commandsCount[0] - commandsLeft];
			}
			else
			{

				Console.Write(">");
				string input = Console.ReadLine();
				int indexOfPipe = input.IndexOf('|');
				realCommand[0] = input;
				if (indexOfPipe >= 0)
				{
					string[] newCommands = input.Trim().Split('|');
					
					commandsLeft = newCommands.Length;
					for (int i = 0; i < newCommands.Length; i++)
					{
						commands[i] = newCommands[i];
					}
					commandsCount[0] = newCommands.Length;
					realCommand[0] = commands[0];
					
				}
			}
			return commandsLeft;
		}
	}
}
