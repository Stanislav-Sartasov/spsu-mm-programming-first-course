using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthTask.BashDescription
{
	public class Bash
	{
		internal static readonly string[] commands = { "echo", "exit", "pwd", "cat", "wc" };

		public Parser Parser { get; set; } = new Parser();
		public static Values Values { get; set; } = new Values();

		public static string RunCommand(int commandNum, string str)
		{
			var resStr = "";
			switch (commandNum)
			{
				case 0:
					var echo = new CommandEcho();
					echo.RunCommand(str, Values);
					break;
				case 1:
					var exit = new CommandExit();
					exit.RunCommand(str);
					break;

				case 2:
					var pwd = new CommandPwd();
					pwd.RunCommand(str);
					break;
				case 3:
					var cat = new CommandCat();
					cat.RunCommand(str);
					break;

				case 4:
					var wc = new CommandWc();
					wc.RunCommand(str);
					break;
			}

			return resStr;
		}

		public void Start()
		{
			while (true)
			{
				var str = Console.ReadLine();
				Parser.Parse(str, Values);
			}
		}
	}
}