using System;

namespace BashDescription.Commands
{
	public class CommandExit : Command
	{
		public override void RunCommand()
		{
			try
			{
				if (Input != "")
				{
					throw new Exception("Invalid input.");
				}

				Environment.Exit(0);
			}
			catch (Exception ex)
			{
				Output = ex.Message;
			}
		}
		public CommandExit(string input) : base(input)
		{
			Input = input;
		}
	}
}