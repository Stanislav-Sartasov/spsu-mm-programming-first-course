using System;

namespace BashDescription.Commands
{
	public class CommandEcho : Command
	{
		public override void RunCommand()
		{
			Output = Input;
		}

		public CommandEcho(string input) : base(input)
		{
			Input = input;
		}
	}
}