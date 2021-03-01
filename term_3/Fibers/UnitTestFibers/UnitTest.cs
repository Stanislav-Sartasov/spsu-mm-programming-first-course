using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FiberLib;

namespace UnitTestFibers
{
    [TestClass]
    public class UnitTest
    {
        private int amountProcess;

        [TestInitialize]
        public void ProcessesInit()
        {
            amountProcess = 3;
            for (int i = 0; i < amountProcess; i++)
            {
                Process process = new Process();
                ProcessManager.AddProcess(process);
            }
        }
        [TestMethod]
        public void PrioritiTest()
        {
            ProcessManager.Exec(SchedulerPriority.PriorityLevel);
            ProcessManager.Dispose();
        }

        [TestMethod]
        public void NonePriorityTest()
        {
            ProcessManager.Exec(SchedulerPriority.NonePriority);
            ProcessManager.Dispose();
        }
    }
}