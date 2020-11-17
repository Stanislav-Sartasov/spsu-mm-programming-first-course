using System.IO;

namespace Task10
{
	class Cat : ICommand
	{
		public string Process(string input, out CommandHandler.Keys key)
		{
			if (!File.Exists(input))
			{
				key = CommandHandler.Keys.Error;
				return input != null && input != "" ? "File \"" + input + "\" does not exist" : "File name missing";
			}

			try
			{
				string output = File.ReadAllText(input);
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