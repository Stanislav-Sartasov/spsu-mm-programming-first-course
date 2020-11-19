using Fibers.Framework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace Fibers.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestInitialize()
        {
            List<Process> processes = new List<Process>();
            for (int i = 0; i < 10; i++)
                processes.Add(new Process());

            bool actual = ProcessManager.Initialize(processes);
            Assert.IsTrue(actual);
            ProcessManager.Dispose();
        }

        [TestMethod]
        public void TestRun()
        {
            List<Process> processes = new List<Process>();
            for (int i = 0; i < 3; i++)
                processes.Add(new Process());

            ProcessManager.Initialize(processes, true);
            ProcessManager.Run();
            var actual = ProcessManager.GetDelList();
            Assert.AreEqual(3, actual.Count);
            ProcessManager.Dispose();
        }

        [TestMethod]
        public void TestLcmFirst()
        {
            List<int> input = new List<int>() { 1, 2, 3 };
            Assert.AreEqual(6, MyMath.ListOfLcm(input)[2]);
        }

        [TestMethod]
        public void TestLcmSecond()
        {
            List<int> input = new List<int>() { 2, 4, 6 };
            Assert.AreEqual(12, MyMath.ListOfLcm(input)[2]);
        }

        [TestMethod]
        public void TestGcdFirst()
        {
            List<int> input = new List<int>() { 1, 2, 3 };
            Assert.AreEqual(1, MyMath.Gcd(input));
        }

        [TestMethod]
        public void TestGcdSecond()
        {
            List<int> input = new List<int>() { 2, 4, 6 };
            Assert.AreEqual(2, MyMath.Gcd(input));
        }

        [TestMethod]
        public void TestGcdThird()
        {
            int a = 2;
            int b = 4;
            Assert.AreEqual(2, MyMath.Gcd(a, b));
        }

        [TestMethod]
        public void TestGcdFourth()
        {
            int a = 4;
            int b = 2;
            Assert.AreEqual(2, MyMath.Gcd(a, b));
        }
    }
}
