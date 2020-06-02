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
        Bash bash;
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IBashController> catMock = new Mock<IBashController>();
            catMock.SetupSequence(c => c.GetCommand()).Returns("cat" + ' ' + filePath).Returns("exit");
            bash = new Bash(catMock.Object, s => result.Add(s));
            bash.Start();
        }

        [TestMethod]
        public void CorrectCat()
        {
            for (int i = 0; i < 15; i++)
                Assert.AreEqual("line" + (i + 1), result[i + 2]);
        }
    }
}
