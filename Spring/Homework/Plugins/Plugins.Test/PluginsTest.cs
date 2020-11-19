using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interface;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.Test
{
	[TestClass]
	public class PluginsTest
	{
		static readonly string path = string.Format("{0}Implement\\bin", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")));

		Finder intFinder;
		Finder strFinder;
		Type strType = typeof(Interface<string>);
		Type intType = typeof(Interface<int>);
		IEnumerable<Interface<string>> implementingStrClasses;
		IEnumerable<Interface<int>> implementingIntClasses;

		[TestInitialize]
		public void PluginsTestsInit()
		{
			try
			{
				strType = typeof(Interface<string>);
				intType = typeof(Interface<int>);
				strFinder = new Finder(strType, path);
				intFinder = new Finder(intType, path);
				implementingStrClasses = strFinder.GetImplementingClasses().Select(obj => (Interface<string>)obj);
				implementingIntClasses = intFinder.GetImplementingClasses().Select(obj => (Interface<int>)obj);
			}
			catch
			{
				throw new Exception("Initialization error");
			}
		}

		[TestMethod]
		public void CorrectNumberOfStrClasses()
		{
			Assert.AreEqual(1, implementingStrClasses.Count());
		}

		[TestMethod]
		public void CorrectNumberOfIntClasses()
		{
			Assert.AreEqual(1, implementingIntClasses.Count());
		}

		[TestMethod]
		public void CorrectStrClassFunctionality()
		{
			foreach (var cl in implementingStrClasses)
			{
				cl.Set("correct");
				Assert.AreEqual("correct", cl.Get());
				Assert.AreEqual("String class", cl.GetInfo());
			}
		}

		[TestMethod]
		public void CorrectIntClassFunctionality()
		{
			foreach (var cl in implementingIntClasses)
			{
				cl.Set(1);
				Assert.AreEqual(1, cl.Get());
				Assert.AreEqual("Int class", cl.GetInfo());
			}
		}
	}
}
