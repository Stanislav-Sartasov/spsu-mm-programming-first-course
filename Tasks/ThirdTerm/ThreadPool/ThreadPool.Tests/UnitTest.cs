using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadPool;

namespace ThreadPool.Tests
{
    [TestClass]
    public class UnitTest
    {
        private object locker = new object();
        private volatile int count = 0;

        [TestMethod]
        public void TestMethod()
        {
            ThreadPool threadPool = new ThreadPool();
            Action action = SomeWork;
            for (int i = 0; i < 25; i++)
            {
                threadPool.Enqueue(action);
                Thread.Sleep(0);
            }
            threadPool.Dispose();

            Assert.AreEqual(25, count);
            Assert.AreEqual(0, threadPool.GetTasksCount());
            Assert.AreEqual(0, threadPool.GetThreadsCount());
        }

        private void SomeWork()
        {
            lock (locker)
                count++;
        }
    }
}
