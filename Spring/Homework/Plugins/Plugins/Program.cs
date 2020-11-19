using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interface;

namespace Plugins
{
	class Program
	{
		public static string CorrectInput()
		{
			while (true)
			{
				Console.WriteLine("Enter the path to the directory containing the dll file:\n");
				string path = Console.ReadLine();
				if (Directory.Exists(path))
				{
					return path;
				}
				else
					Console.WriteLine("This directory doesn't exist! Try again!\n");
			}
		}

		static void Main(string[] args)
		{
			var type = typeof(Interface<string>);
			var finder = new Finder(type, CorrectInput());

			IEnumerable<Interface<string>> implementingСlasses = finder.GetImplementingClasses().Select(obj => (Interface<string>)obj);

			if (implementingСlasses != null)
			{
				foreach (var implementingClass in implementingСlasses)
				{
					try
					{
						implementingClass.Set("string");
						Console.WriteLine(implementingClass.Get());
						Console.WriteLine(implementingClass.GetInfo());
					}
					catch
					{
						throw new Exception();
					}
				}
			}
		}
	}
}
