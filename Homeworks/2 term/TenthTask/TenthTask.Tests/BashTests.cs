using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TenthTask.BashDescription;

namespace TenthTask.Tests
{
	[TestClass]
	public class BashTests
	{
		Bash bash = new Bash();
		[TestMethod]
		public void EchoTest()
		{
			string input = "echo fdddf";
			bash.Parser.Parse(input);
		}
	}
}
