using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bash.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bash.Tests
{
    [TestClass]
    public class StringsTest
    {
        [TestMethod]
        public void TestInsert()
        {
            string argument = "   aaa  $a sdf    $b $a$b asdf";
            Dictionary<string, string> variables = new Dictionary<string, string>();
            variables["$a"] = "4";
            variables["$b"] = "5";
            string actual = Strings.InsertVar(argument, variables);
            string expected = "   aaa  4 sdf    5 45 asdf";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCountWords()
        {
            string argument = "   aaa  $a sdf    $b $a$b asdf";
            int actual = Strings.CountWords(argument);
            int expected = 6;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCountLines()
        {
            string argument = "   aaa\n  $a sdf    $b $a$b asdf\n";
            int actual = Strings.CountLines(argument);
            int expected = 2;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSplit()
        {
            string argument = "   aaa  $a sdf    $b $a$b asdf";
            string actual = Strings.SplitToCommand(argument);
            string expected = "aaa";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRemoveFirstWord()
        {
            string argument = "   aaa  $a sdf    $b $a$b asdf";
            string actual = Strings.RemoveFirstWord(argument);
            string expected = "$a sdf    $b $a$b asdf";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestChooseCommand()
        {
            string argument = "echo   aaa  $a sdf    $b $a$b asdf";
            ICommand actual = Strings.ChooseCommand(argument);
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void TestRemoveToFirstSymbol()
        {
            string argument = "   aaa  $a sdf    $b $a$b asdf";
            string actual = Strings.RemoveToFirstSymbol(argument, '$').ToString();
            string expected = "a sdf    $b $a$b asdf";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSubstringToSymbol()
        {
            string argument = "   aaa  $a sdf    $b $a$b asdf";
            string actual = Strings.SubstringToSymbol(argument, '$').ToString();
            string expected = "aaa";
            Assert.AreEqual(expected, actual);
        }
    }
}
