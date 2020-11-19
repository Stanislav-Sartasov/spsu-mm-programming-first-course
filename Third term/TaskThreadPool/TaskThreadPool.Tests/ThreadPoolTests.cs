using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadPool;

namespace TaskThreadPool.Tests
{
    [TestClass]
    public class ThreadPoolTests
    {
        ThreadPool.ThreadPool threadPool = new ThreadPool.ThreadPool();
        List<string> output = new List<string>();
        private int count;
        [TestMethod]
        public void TestThreadPool()
        {
            
            Action action = DoSomething;
            for (int i = 0; i < 100; i++)
            {
                threadPool.Enqueue(action);
                Thread.Sleep(10);
            }
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(output.Contains($"finished {i}"));
            }
            threadPool.Dispose();
            Assert.AreEqual(threadPool.GetTasksCount(), 0);
            Assert.AreEqual(threadPool.GetThreadsCount(), 0);
        }
        private void DoSomething()
        {
            output.Add($"finished {count}");
            count++;
        }
    }
}
