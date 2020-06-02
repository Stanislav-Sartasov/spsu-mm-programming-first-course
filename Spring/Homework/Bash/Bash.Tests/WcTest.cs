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
        Bash bash;
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IBashController> wcMock = new Mock<IBashController>();
            wcMock.SetupSequence(c => c.GetCommand()).Returns("wc" + ' ' + filePath).Returns("exit");
            bash = new Bash(wcMock.Object, s => result.Add(s));
            bash.Start();
        }

        [TestMethod]
        public void CorrectWc()
        {
            if (result.Count >= 5)
            {
                Assert.AreEqual("Number of lines: 16", result[2]);
                Assert.AreEqual("Number of words: 16", result[3]);
                Assert.AreEqual("Number of bytes: 113", result[4]);
            }
        }
    }
}
