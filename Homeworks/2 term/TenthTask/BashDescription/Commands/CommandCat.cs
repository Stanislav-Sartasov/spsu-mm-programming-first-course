using System;
using System.IO;

namespace BashDescription.Commands
{
	public class CommandCat : Command
	{
		public override void RunCommand()
		{
			try
			{
				var lines = File.ReadAllText(Input);

				Output = String.Concat(lines);
			}
			catch (ArgumentException)
			{
				Output = "File not found.";
			}
			catch (FileNotFoundException)
			{
				Output = "File not found.";
			}
			catch (Exception ex)
			{
				Output = ex.Message;
			}
		}

		public CommandCat(string input) : base(input)
		{
			Input = input;
		}
	}
}