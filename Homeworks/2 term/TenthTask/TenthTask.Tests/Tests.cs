using Microsoft.VisualStudio.TestTools.UnitTesting;
using BashDescription;
using BashDescription.Commands;

namespace TenthTask.Tests
{
	[TestClass]
	public class Tests // in progress
	{
		[TestMethod]
		public void TestOperator()
		{
			var input = "$a = 4";

			var parser = new Parser();
			
			var result = parser.Parse(input);

			Assert.AreEqual("4", result[0]);
		}

		[TestMethod]
		public void TestEcho()
		{
			var input = "echo 44443fvffgf g f f  ";

			var parser = new Parser();

			var result = parser.Parse(input);

			Assert.AreEqual("44443fvffgf g f f", result[1]);
		}
	}
}
