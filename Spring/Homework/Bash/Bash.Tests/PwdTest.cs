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
        static readonly string filePath = string.Format("{0}\\test.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")));
        Bash bash;
        List<string> result;
        string directory;
        string[] dirFiles;

        [TestInitialize]
        public void TestInit()
        {
            result = new List<string>();
            Mock<IBashController> pwdMock = new Mock<IBashController>();
            pwdMock.SetupSequence(c => c.GetCommand()).Returns("pwd").Returns("exit");
            bash = new Bash(pwdMock.Object, s => result.Add(s));
            bash.Start();
            directory = Directory.GetCurrentDirectory();
            dirFiles = Directory.GetFiles(directory);
        }

        [TestMethod]
        public void CorrectPwd()
        {
            Assert.AreEqual(directory, result[2]);
            for (int i = 0; i < dirFiles.Length; i++)
                Assert.AreEqual(dirFiles[i], result[3 + i]);
        }
    }
}
