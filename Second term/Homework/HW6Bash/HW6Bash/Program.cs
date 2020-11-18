using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace HW6Bash
{
	class Program
	{

		static void Main(string[] args)
		{
			const int MAXCOMMANDS = 10;
			const int MAXVARS = 50;
			int[] commandsCount = {0};
			string[] localNames = new string[MAXVARS];
			string[] localValues = new string[MAXVARS];
			int totalVars = 0;
			string[] buffer = { "" };
			int commandsLeft = 0;

			string[] commands = new string[MAXCOMMANDS];
			string[] realCommand = { null };
			bool stopFlag = false;
			while (!stopFlag)
			{
				commandsLeft = PhaseOneCommand.Execute(commandsLeft, realCommand, commands, commandsCount);

				string[] s = realCommand[0].Trim().Split(' ');
				switch (s[0])
				{
					case "echo":

						EchoCommand.Execute(s, buffer, localNames, localValues, totalVars);
						commandsLeft--;
						break;

					case "pwd":
						PwdCommand.Execute();
						break;

					case "cat":
						CatCommand.Execute(s, buffer);
						commandsLeft--;
						break;

					case "wc":
						WcCommand.Execute(s);
						break;

					case "exit":
						stopFlag = true;
						break;

					default: // пробуем определить заведение новой переменной
						totalVars = DefaultCommand.Execute(s, localNames, localValues, totalVars, MAXVARS);
						break;
				}
			}
		}
	}
}
