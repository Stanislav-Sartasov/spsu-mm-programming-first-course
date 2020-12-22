using System;
using System.Collections.Generic;
using BashDescription.Commands;

namespace BashDescription
{
	public static class Bash
	{
		public static Dictionary<string, string> Variables { get; private set; } = new Dictionary<string, string>();
		public static Parser Parser { get; private set; } = new Parser();

		public static void Start()
		{
			Info();

			while (true)
			{
				try
				{
					var str = Console.ReadLine().Trim();

					if (str == "")
					{
						throw new Exception("Please, write the command.");
					}

					List<Command> listOfCommands = Parser.Parse(str);

					string output = "";
					foreach (var command in listOfCommands)
					{
						if (command.Input == "")
						{
							command.Input = output;
						}

						command.RunCommand();

						output = command.Output;
						Console.WriteLine(output);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		private static void Info()
		{
			Console.WriteLine("Welcome to bash!\nCommands: \"echo [argument]\", \"exit\", \"pwd\", \"cat [filename]\", \"wc [filename]\".\nYou can also use operators \"$\" and \"|\".");
		}
	}
}
