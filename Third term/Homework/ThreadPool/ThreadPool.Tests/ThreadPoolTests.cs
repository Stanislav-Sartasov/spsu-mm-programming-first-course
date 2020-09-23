using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadPool.Tests
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
                for (int i = 0; i < numOfTasks; i++)
                {
                    var num = i;
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
