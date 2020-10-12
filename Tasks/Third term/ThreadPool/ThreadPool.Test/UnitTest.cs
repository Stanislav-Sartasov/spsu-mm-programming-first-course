using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Threading;

namespace ThreadPool.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestTasks()
        {
            Library.ThreadPool threadPool = new Library.ThreadPool();
            threadPool.Start();
            int numberOfTasks = 20;
            for (int i = 0; i < numberOfTasks; i++)
                threadPool.Enqueue(new Action(SomeAction));
            threadPool.Stop();
            var actualTasks = threadPool.GetTasks();
            Assert.IsTrue(0 <= (numberOfTasks - actualTasks.Count));
            threadPool.Dispose();
        }

        [TestMethod]
        public void TestWork()
        {
            Library.ThreadPool threadPool = new Library.ThreadPool();
            threadPool.Start();
            int numberOfTasks = 20;
            for (int i = 0; i < numberOfTasks; i++)
                threadPool.Enqueue(new Action(SomeAction));
            threadPool.Stop();
            var actualTasksOne = threadPool.GetTasks();
            threadPool.Continue();
            Thread.Sleep(500);
            threadPool.Stop();
            var actualTasksTwo = threadPool.GetTasks();
            Assert.IsTrue(0 <= (actualTasksOne.Count - actualTasksTwo.Count));
            threadPool.Dispose();
        }

        [TestMethod]
        public void TestThreads()
        {
            Library.ThreadPool threadPool = new Library.ThreadPool();
            threadPool.Start();
            int expected = 5;
            
            var actualThreads = threadPool.GetThreads();
            Assert.AreEqual(expected, actualThreads.Count);
            threadPool.Dispose();
        }

        private void SomeAction()
        {
            Console.WriteLine("Starting to work");
            Thread.Sleep(new Random().Next(1, 1000));
            Console.WriteLine("finishing work\n");
        }
    }
}
