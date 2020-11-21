using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using InterfacesDescription;

namespace SixthTask.Tests
{
	[TestClass]
	public class PluginTests
	{
		private string Path { get; set; }
		private int[] Test { get; set; }

		[TestMethod]
		public void CorrectInputTest()
		{
			Path = String.Concat(AppDomain.CurrentDomain.BaseDirectory, "..", @"\..", @"\..\", "TestSources"); // default .dll directory

			SearchByPath(Path);

			Assert.AreEqual(2, Test[0]);
			Assert.AreEqual(5, Test[1]);

		}

		[TestMethod]
		public void IncorrectInputTest()
		{
			Path = String.Concat(AppDomain.CurrentDomain.BaseDirectory, "..", @"\..", @"\.."); // no .dll files

			SearchByPath(Path);

			Assert.AreEqual(null, Test); // caught an expression, test array was not initialized
		}

		private void SearchByPath(string Path)
		{
			try
			{
				var dir = new DirectoryInfo(Path).GetFiles("*.dll");
				var plugins = new List<IPlugin>();

				for (int i = 0; i < dir.Length; i++)
				{
					var pluginTypes = new List<Type>();
					var types = Assembly.LoadFile(dir[i].FullName).GetTypes();

					foreach (var searchType in types)
					{
						if (searchType.GetInterfaces().Contains(typeof(IPlugin)))
						{
							pluginTypes.Add(searchType);
						}
					}

					foreach (var type in types)
					{
						var plugin = Activator.CreateInstance(type) as IPlugin;
						plugins.Add(plugin);
					}
				}

				if (plugins.Count != 0)
				{
					Test = new int[2];
					int j = 0;
					foreach (var plugin in plugins)
					{
						//Debug.WriteLine(plugin.Operation(2, 5));
						Test[j] = plugin.Operation(2, 5);
						j++;
					}
				}
				else 
				{
					Debug.WriteLine("Plugins not found!");
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}