using System.Collections;
using System.Collections.Generic;

namespace Task10
{
	public class CommandHandler
	{
		public CommandHandler()
		{
			commandTable["echo"] = new Echo();
			commandTable["exit"] = new Exit();
			commandTable["pwd"] = new Pwd();
			commandTable["cat"] = new Cat();
			commandTable["wc"] = new Wc();
			commandTable["$"] = new VariableHandler(variables);
		}

		public enum Keys
		{
			Ok,
			Exit,
			Error
		}

		Hashtable variables = new Hashtable();
		InputHandler inputHandler = new InputHandler();
		string preCommandOut = null;
		string oldPreCommandOut = null;
		Hashtable commandTable = new Hashtable();

		public string Process(string input, out Keys keyOut)
		{
			return Process(inputHandler.Parse(input), out keyOut);
		}

		public string Process(List<Command> commands, out Keys keyOut)
		{
			if (commands == null)
			{
				keyOut = Keys.Ok;
				return null;
			}			
			NewCommandInput();
			Keys key = Keys.Error;
			string commandOut = "";
			foreach (Command i in commands)
			{
				commandOut = Process(i, out key);
				if (key != Keys.Ok)
					break;
			}
			keyOut = key;
			return commandOut;
		}

		public string Process(Command command, out Keys keyOut)
		{
			if (command.Name == "pipe")
			{
				preCommandOut = oldPreCommandOut;
				keyOut = Keys.Ok;
				return null;
			}
			
			string argument = command.Argument;
			if (preCommandOut != null)
				argument += preCommandOut;

			while (argument.Contains('$'))
			{
				int pos = argument.LastIndexOf('$');
				string variableName = "";
				for (int i = pos + 1; i < argument.Length; i++)
				{
					if ((argument[i] >= 'a' && argument[i] <= 'z') || (argument[i] >= 'A' && argument[i] <= 'Z') || (argument[i] >= '0' && argument[i] <= '9') || argument[i] == '_')
						variableName += argument[i];
					else
						break;
				}

				if (variableName == "")
				{
					keyOut = Keys.Error;
					return OutStringParse(command, Keys.Error, "Invalid variable name: a..z, A..Z, 0..9, _ should be used");
				}

				var value = variables[variableName];

				if (value == null)
				{
					keyOut = Keys.Error;
					return OutStringParse(command, Keys.Error, "Variable \"" + variableName + "\" is not defined");
				}

				argument = argument[0..pos] + (string)value + argument.Substring(pos + variableName.Length + 1);
			}

			ICommand commandExecutor = (ICommand)commandTable[command.Name];
			if (commandExecutor == null)
				commandExecutor = new Unidentified();

			string commandOut = commandExecutor.Process(argument, out Keys key);

			if (command.Argument != "" && key == Keys.Error && preCommandOut != null && command.Name == "unidentified")
				commandOut = commandExecutor.Process(argument + " " + preCommandOut, out key);

			preCommandOut = commandOut;


			keyOut = key;
			return OutStringParse(command, key, commandOut);
		}

		string OutStringParse(Command command, Keys key, string commandOut)
		{
			string outString = null;

			if (commandOut != null && key == Keys.Ok)
				outString = commandOut;
			else if (key == Keys.Error)
			{
				if (command.Name == "$")
					outString = "\"" + command.Name + command.Argument;
				else if (command.Name == "unidentified")
					outString = "\"" + command.Argument;
				else
					outString = "\"" + command.Name + " " + command.Argument;

				outString += "\" command can not process";
				if (commandOut != null)
					outString += "\n" + commandOut;
			}

			return outString;
		}

		public void NewCommandInput()
		{
			oldPreCommandOut = preCommandOut;
			preCommandOut = null;
		}
	}
}
