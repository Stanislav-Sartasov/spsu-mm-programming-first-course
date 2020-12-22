using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using BashDescription;
using BashDescription.Commands;

namespace TenthTask.Tests
{
	[TestClass]
	public class BashTests
	{
		Parser parser = new Parser();

		[TestMethod]
		public void TestEcho()
		{
			var input = "  echo 44443fvffgf g f f  ";

			var result = parser.Parse(input);
			result[0].RunCommand();

			Assert.AreEqual("44443fvffgf g f f", result[0].Output);
		}

		[TestMethod]
		public void TestOperator()
		{
			var input = "$a = 4";
			parser.Parse(input);

			var result = parser.Parse("echo $a");
			result[0].RunCommand();

			Assert.AreEqual("4", result[0].Output); // Variable a equals 4
		}

		[TestMethod]
		public void TestCat()
		{
			CreateText("1.txt", "test");

			var input = " cat 1.txt  ";

			var result = parser.Parse(input);
			result[0].RunCommand();

			Assert.AreEqual("test", result[0].Output);
			File.Delete("1.txt");
		}

		[TestMethod]
		public void TestPwd()
		{
			CreateText("test.txt");

			var input = " pwd   ";

			var result = parser.Parse(input);
			result[0].RunCommand();

			Assert.IsTrue(result[0].Output.Contains("test.txt"));
			File.Delete("test.txt");
		}

		[TestMethod]
		public void TestWc()
		{
			CreateText("1.txt", "The cake is a lie");

			var input = "         wc 1.txt  ";

			var result = parser.Parse(input);
			result[0].RunCommand();

			Assert.AreEqual("1 lines\n5 words\n17 bytes", result[0].Output);
			File.Delete("1.txt");
		}

		[TestMethod]
		public void TestConvTwoCommands()
		{
			CreateText("1.txt", "The cake is a lie");

			var input = " echo 1.txt | wc";

			var result = parser.Parse(input);

			//Bash.Start() emulating (feature of this test)
			string inputSec = "";
			foreach (Command cmd in result)
			{
				if (cmd.Input == "")
				{
					cmd.Input = inputSec;
				}

				cmd.RunCommand();
				inputSec = cmd.Output; 
			}
			//

			Assert.AreEqual("1.txt", result[0].Output);
			Assert.AreEqual("1 lines\n5 words\n17 bytes", result[1].Output);
			File.Delete("1.txt");
		}

		[TestMethod]
		public void TestExit()
		{
			var input = "exit ";

			var result = parser.Parse(input);
			//result[0].RunCommand();

			Assert.AreEqual("", result[0].Input);  // Exit doesn't get any value
		}
		
		private void CreateText(string path, string text = "")
		{
			using (StreamWriter file = File.CreateText(path))
			{
				if (file == null)
				{
					throw new Exception();
				}

				file.Write(text);
			}
		}
	}
}
