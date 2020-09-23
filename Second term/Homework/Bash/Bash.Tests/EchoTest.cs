using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bash.Tests
{
    [TestClass]
    public class EchoTest
    {
        static readonly string filePath = string.Format("{0}\\test.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")));
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IController> echoController = new Mock<IController>();
            echoController.SetupSequence(c => c.GetCommand())
                .Returns("$path=" + filePath)
                .Returns("echo " + "$path")
                .Returns("exit");
            Mock<IPrinter> echoPrinter = new Mock<IPrinter>();
            echoPrinter.Setup(s => s.Print(It.IsAny<string>()))
                .Callback((string str) =>
                {
                    result.Add(str);
                }).Verifiable();
            Bash.BashController = echoController.Object;
            Bash.Printer = echoPrinter.Object;
            Bash.Start();
        }

        [TestMethod]
        public void CorrectEcho()
        {
            Assert.AreEqual(filePath, result[2]);
        }
    }
}
