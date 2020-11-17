using System;
using System.IO;

namespace Task10
{
	class Wc : ICommand
	{
		public string Process(string input, out CommandHandler.Keys key)
		{
			if (!File.Exists(input))
			{
				key = CommandHandler.Keys.Error;
				return "File \"" + input + "\" does not exist";
			}

			string output;
			try
			{
				output = input + ":\nlines: " + File.ReadAllLines(input).Length + "; words: " + File.ReadAllText(input).Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length + "; bytes: " + File.ReadAllBytes(input).Length;
				key = CommandHandler.Keys.Ok;
				return output;
			}
			catch
			{
				key = CommandHandler.Keys.Error;
				return null;
			}
		}
	}
}
