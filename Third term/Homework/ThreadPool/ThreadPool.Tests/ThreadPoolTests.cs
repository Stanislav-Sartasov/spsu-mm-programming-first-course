using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace ThreadPoolLib.Tests
{
    [TestClass]
    public class ThreadPoolTests
    {
        static int numOfTasks = 1000;
        private int[] results = new int[numOfTasks];

        [TestInitialize]
        public void Init()
        {
            using (var tp = new ThreadPool())
            {
                for (int i = 0; i < 1000; i++)
                {
                    int num = i;
                    Thread.Sleep(1);
                    tp.Enqueue(() => results[num] = num * num);
                }
            }
        }

        [TestMethod]
        public void CorrectTaskSolution()
        {
            for (int i = 0; i < numOfTasks; i++)
                Assert.AreEqual(i * i, results[i]);
        }
    }
}
