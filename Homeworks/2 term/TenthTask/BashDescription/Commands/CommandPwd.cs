using System;

namespace BashDescription.Commands
{
	public class CommandPwd : Command
	{
		public override void RunCommand()
		{
			try
			{
				if (Input != "")
				{
					throw new Exception("Invalid input.");
				}

				Output = String.Join("\n", System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory()));	
			}
			catch (Exception ex)
			{
				Output = ex.Message;
			}
		}

		public CommandPwd(string input) : base(input)
		{
			Input = input;
		}
	}
}