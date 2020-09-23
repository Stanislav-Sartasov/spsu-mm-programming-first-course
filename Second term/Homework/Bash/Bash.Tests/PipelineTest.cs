using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bash.Tests
{
    [TestClass]
    public class PipelineTest
    {
        static readonly string filePath = string.Format("{0}\\test.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")));
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IController> pipeController = new Mock<IController>();
            pipeController.SetupSequence(c => c.GetCommand())
                .Returns("echo" + ' ' + filePath + '|' + " wc")
                .Returns("exit");
            Mock<IPrinter> pipePrinter = new Mock<IPrinter>();
            pipePrinter.Setup(s => s.Print(It.IsAny<string>()))
                .Callback((string str) =>
                {
                    result.Add(str);
                }).Verifiable();
            Bash.BashController = pipeController.Object;
            Bash.Printer = pipePrinter.Object;
            Bash.Start();
        }

        [TestMethod]
        public void CorrectPipeline()
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
