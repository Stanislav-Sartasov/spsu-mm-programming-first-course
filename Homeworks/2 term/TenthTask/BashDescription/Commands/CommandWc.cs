using System;
using System.IO;
using System.Linq;

namespace BashDescription.Commands
{
	public class CommandWc : Command
	{
		public override void RunCommand()
		{
			try
			{
				var numOfBytes = String.Concat(File.ReadAllBytes(Input).Count().ToString(), " bytes");

				var numOfLines = String.Concat(File.ReadLines(Input).Count().ToString(), " lines\n");

				var numOfWords = String.Concat(File.ReadAllText(Input).Split(' ', StringSplitOptions.RemoveEmptyEntries).Count().ToString(), " words\n");

				Output = String.Concat(numOfLines, numOfWords, numOfBytes);
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
		public CommandWc(string input) : base(input)
		{
			Input = input;
		}
	}
}