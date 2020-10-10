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
    }
}
