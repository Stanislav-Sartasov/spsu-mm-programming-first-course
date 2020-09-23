using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bash.Tests
{
    [TestClass]
    public class WcTest
    {
        static readonly string filePath = string.Format("{0}\\test.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")));
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IController> wcController = new Mock<IController>();
            wcController.SetupSequence(c => c.GetCommand()).Returns("wc " + filePath).Returns("exit");
            Mock<IPrinter> wcPrinter = new Mock<IPrinter>();
            wcPrinter.Setup(s => s.Print(It.IsAny<string>()))
                .Callback((string str) =>
                {
                    result.Add(str);
                }).Verifiable();
            Bash.BashController = wcController.Object;
            Bash.Printer = wcPrinter.Object;
            Bash.Start();
        }

        [TestMethod]
        public void CorrectWc()
        {
            var lines = File.ReadAllLines(filePath);
            int wordsCount = 0;
            foreach (string line in lines)
            {
                wordsCount += (line.Trim().Split(' ')).Length;
            }
            var bytesCount = new FileInfo(filePath).Length;

            Assert.AreEqual(filePath + ':', result[2]);
            Assert.AreEqual("Number of lines: " + lines.Length, result[3]);
            Assert.AreEqual("Number of words: " + wordsCount, result[4]);
            Assert.AreEqual("Number of bytes: " + bytesCount, result[5]);
        }
    }
}
