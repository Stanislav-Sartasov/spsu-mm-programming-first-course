using System;
using System.Diagnostics;

namespace BashDescription.Commands
{
	public class Command
	{
		public virtual void RunCommand()
		{
			try
			{
				var cmd = Input.Split(' ');

				if (cmd.Length == 1)
				{
					Process.Start(Input);
				}
				else
				{
					Process.Start(cmd[0], String.Join(null, cmd, 1, cmd.Length - 1));
				}

				Output = "Undefined command, starting System.Process().";
			}
			catch
			{
				Output = "Invalid input.";
			}
		}

		public string Input { get; protected set; }
		public string Output { get; protected set; }

		public Command(string input)
		{
			Input = input;
		}
	}
}