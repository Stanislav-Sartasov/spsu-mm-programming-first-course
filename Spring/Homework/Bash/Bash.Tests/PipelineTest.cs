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
        Bash bash;
        List<string> result;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IBashController> echoMock = new Mock<IBashController>();
            echoMock.SetupSequence(c => c.GetCommand())
                .Returns("$h=7" + ' ' + '|' + "echo" + ' ' + "$h")
                .Returns("exit");
            bash = new Bash(echoMock.Object, s => result.Add(s));
            bash.Start();
        }

        [TestMethod]
        public void CorrectPipeline()
        {
            Assert.AreEqual("7", result[2]);
        }
    }
}
