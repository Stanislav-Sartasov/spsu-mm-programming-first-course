using System;
using System.Collections.Generic;
using System.Text;
using BashLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserLibrary;

namespace Task6Bash.Tests
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void TestGetCommand()
        {
            string input = "       echo      test1  ";
            Assert.AreEqual(Parser.GetCommand(input), "echo");
        }
        [TestMethod]
        public void TestGetArguments()
        {
            string input = "     echo test1   test2      ";
            Console.WriteLine(Parser.GetArguments(input));
            Assert.AreEqual(Parser.GetArguments(input), "test1 test2");
        }
        [TestMethod]
        public void TestParsePipeline()
        {
            string input = "aa bbb|ddd";
            int index = Parser.ParsePipeline(input, 0);
            Assert.AreEqual(input.IndexOf('|'), Parser.ParsePipeline(input, 0));
        }
        [TestMethod]
        public void TestParseLocalVariable()
        {
            string input = "$path=C:\\Program Files";
            KeyValuePair<string, string> pair = Parser.ParseLocalVariable(input);
            Assert.AreEqual(pair.Key, "$path");
            Assert.AreEqual(pair.Value, "C:\\Program Files");
        }
    }
}
