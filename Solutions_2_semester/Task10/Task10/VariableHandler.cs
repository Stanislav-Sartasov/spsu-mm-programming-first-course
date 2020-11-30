using System;
using System.Collections;
using System.Linq;

namespace Task10
{
	class VariableHandler : ICommand
	{
		public VariableHandler(Hashtable variables)
		{
			this.variables = variables;
		}

		Hashtable variables;

		public string Process(string input, out CommandHandler.Keys key)
		{
			string variableName;
			string value;
			key = CommandHandler.Keys.Error;

			if (input == "" || input == null)
				return "The variable call should be $varName";

			if (input[0] == ' ')
				return "The variable call should be $varName";

			if (input.Count((x) => x == '=') > 1)
				return "Incorrect operations";

			else if (input.Count((x) => x == '=') == 0)
			{
				variableName = input;
				while (variableName[^1] == ' ')
					variableName = variableName[0..^1];

				foreach (char c in variableName)
					if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '_')
						return "Invalid variable name \"" + variableName + "\": a..z, A..Z, 0..9, _ should be used";

				value = (string)variables[variableName];

				if (value == null)
					return "Variable \"" + variableName + "\" is not defined";
				else
				{
					key = CommandHandler.Keys.Ok;
					return value;
				}
			}

			variableName = input.Substring(0, input.IndexOf('='));
			while (variableName[^1] == ' ')
				variableName = variableName[0..^1];

			foreach (char c in variableName)
				if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '_')
				{
					key = CommandHandler.Keys.Error;
					return "Invalid variable name: a..z, A..Z, 0..9, _ should be used";
				}

			value = input.Substring(input.IndexOf('=') + 1).Replace(" + ", "");
			value = value.Replace("+", "");

			if (value.Length > 0)
				if (value[0] == ' ')
					value = value[1..];

			variables[variableName] = value;

			key = CommandHandler.Keys.Ok;
			return value;
		}
	}
}
