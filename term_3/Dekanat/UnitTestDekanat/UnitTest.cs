using DekanatLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTestDekanat
{
    [TestClass]
    public class UnitTest
    {
        private Random random = new Random();
        private int size = 256;
        [TestMethod]
        public void CuckooHashTest()
        {
            IExamSystem testSystem = new CuckooHash(size);
            Task[] tasks = new Task[8];

            for (int i = 0; i < tasks.Length; i++)
            {
                int span = i * 1100;
                tasks[i] = new Task(() => MakeRequests(span, testSystem));
            }

            foreach (var task in tasks)
            {
                task.Start();
            }

            foreach (var task in tasks)
            {
                task.Wait();
                task.Dispose();
            }
        }

        [TestMethod]
        public void StripedHashTableTest()
        {
            IExamSystem testSystem = new HashTable(size);
            Task[] tasks = new Task[8];

            for (int i = 0; i < tasks.Length; i++)
            {
                int span = i * 1100;
                tasks[i] = new Task(() => MakeRequests(span, testSystem));
            }

            foreach (var task in tasks)
            {
                task.Start();
            }

            foreach (var task in tasks)
            {
                task.Wait();
                task.Dispose();
            }
        }

        private void MakeRequests(int span, IExamSystem examSystem)
        {
            for (int i = 0; i < 1000; i++)
            {
                long student = i + span;
                long course = i + span;

                examSystem.Add(student, course);
                Assert.IsTrue(examSystem.Contains(student, course));

                examSystem.Remove(student, course);
                Assert.IsFalse(examSystem.Contains(student, course));
            }
        }
    }
}
