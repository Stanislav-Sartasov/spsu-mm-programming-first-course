using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Processes;

namespace Fibers.Tests
{
    [TestClass]
    public class FibersTest
    {
        [TestMethod]
        public void InitializeProcesses()
        {
            Assert.IsTrue(ProcessManager.InitializeProcesses(new List<Process>() { new Process() }));
            ProcessManager.Dispose();
        }
    }
}
