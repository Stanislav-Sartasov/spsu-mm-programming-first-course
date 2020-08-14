using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bash.Tests
{
    [TestClass]
    public class PwdTest
    {
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IController> pwdController = new Mock<IController>();
            pwdController.SetupSequence(c => c.GetCommand())
                .Returns("pwd")
                .Returns("exit");
            Mock<IPrinter> pwdPrinter = new Mock<IPrinter>();
            pwdPrinter.Setup(s => s.Print(It.IsAny<string>()))
                .Callback((string str) =>
                {
                    result.Add(str);
                }).Verifiable();
            Bash.BashController = pwdController.Object;
            Bash.Printer = pwdPrinter.Object;
            Bash.Start();
        }

        [TestMethod]
        public void CorrectPwd()
        {
            string directory = Directory.GetCurrentDirectory();
            Assert.AreEqual(Directory.GetCurrentDirectory(), result[2]);
            string[] files = Directory.GetFiles(directory);
            for (int i = 0; i < files.Length; i++)
            {
                Assert.AreEqual(files[i], result[i + 3]);
            }
        }
    }
}
