using System;

namespace Task10
{
	public class MyBash
	{
		CommandHandler commandHandler = new CommandHandler();

		public void Expectation()
		{
			for(;;)
			{
				string output = commandHandler.Process(Console.ReadLine(), out CommandHandler.Keys key);

				if (output != null && output != "")
					Console.WriteLine(output);

				if (key == CommandHandler.Keys.Exit)
					return;
			}
		}
	}
}
