using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bash.Tests
{
    [TestClass]
    public class CatTest
    {
        static readonly string filePath = string.Format("{0}\\test.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")));
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IController> catController = new Mock<IController>();
            catController.SetupSequence(c => c.GetCommand())
                .Returns("cat" + ' ' + filePath)
                .Returns("exit");
            Mock<IPrinter> catPrinter = new Mock<IPrinter>();
            catPrinter.Setup(s => s.Print(It.IsAny<string>()))
                .Callback((string str) => 
                {
                    result.Add(str);
                }).Verifiable();
            Bash.BashController = catController.Object;
            Bash.Printer = catPrinter.Object;
            Bash.Start();
        }

        [TestMethod]
        public void CorrectCat()
        {
            for (int i = 0; i < 15; i++)
                Assert.AreEqual("line" + (i + 1), result[i + 2]);
        }
    }
}
