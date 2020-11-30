using Deanery.System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MutexLockList;

using SpinLockList;

namespace Deanery.Tests
{
    [TestClass]
    public class SystemTest
    {
        [TestMethod]
        public void TestMySpinLockList()
        {
            MySpinLockList<int> myList = new MySpinLockList<int>();
            myList.Add(1);
            myList.Add(2);
            myList.Add(3);

            Assert.IsTrue(myList.Find(1) >= 0);
            Assert.IsTrue(myList.Find(2) >= 0);
            Assert.IsTrue(myList.Find(3) >= 0);

            myList.Remove(1);
            int a = myList.Find(1);
            int b = myList.Find(2);
            int c = myList.Find(3);
            Assert.IsTrue(a == -1);
            Assert.IsTrue(b >= 0);
            Assert.IsTrue(c >= 0);

            myList.Remove(2);
            myList.Remove(3);
            Assert.IsTrue(myList.Find(1) == -1);
            Assert.IsTrue(myList.Find(2) == -1);
            Assert.IsTrue(myList.Find(3) == -1);
        }

        [TestMethod]
        public void TestMyMutexList()
        {
            MyMutexList<int> myList = new MyMutexList<int>();
            myList.Add(1);
            myList.Add(2);
            myList.Add(3);

            Assert.IsTrue(myList.Find(1) >= 0);
            Assert.IsTrue(myList.Find(2) >= 0);
            Assert.IsTrue(myList.Find(3) >= 0);

            myList.Remove(1);
            int a = myList.Find(1);
            int b = myList.Find(2);
            int c = myList.Find(3);
            Assert.IsTrue(a == -1);
            Assert.IsTrue(b >= 0);
            Assert.IsTrue(c >= 0);

            myList.Remove(2);
            myList.Remove(3);
            Assert.IsTrue(myList.Find(1) == -1);
            Assert.IsTrue(myList.Find(2) == -1);
            Assert.IsTrue(myList.Find(3) == -1);
        }


        [TestMethod]
        public void TestListExamSystem()
        {
            IExamSystem mySystem = new ListExamSystem();
            mySystem.Add(1, 1);
            mySystem.Add(2, 2);
            mySystem.Add(3, 1);

            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsTrue(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));
            Assert.IsFalse(mySystem.Contains(3, 3));

            mySystem.Remove(1, 3); // change nothing
            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsTrue(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));

            mySystem.Remove(2, 2);
            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsFalse(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));

            mySystem.Remove(2, 2);
            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsFalse(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));

            mySystem.Remove(1, 1);
            mySystem.Remove(3, 1);
            Assert.IsFalse(mySystem.Contains(1, 1));
            Assert.IsFalse(mySystem.Contains(2, 2));
            Assert.IsFalse(mySystem.Contains(3, 1));
        }

        [TestMethod]
        public void TestMutexExamSystem()
        {
            IExamSystem mySystem = new MutexExamSystem();
            mySystem.Add(1, 1);
            mySystem.Add(2, 2);
            mySystem.Add(3, 1);

            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsTrue(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));
            Assert.IsFalse(mySystem.Contains(3, 3));

            mySystem.Remove(1, 3); // change nothing
            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsTrue(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));

            mySystem.Remove(2, 2);
            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsFalse(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));

            mySystem.Remove(2, 2);
            Assert.IsTrue(mySystem.Contains(1, 1));
            Assert.IsFalse(mySystem.Contains(2, 2));
            Assert.IsTrue(mySystem.Contains(3, 1));

            mySystem.Remove(1, 1);
            mySystem.Remove(3, 1);
            Assert.IsFalse(mySystem.Contains(1, 1));
            Assert.IsFalse(mySystem.Contains(2, 2));
            Assert.IsFalse(mySystem.Contains(3, 1));
        }
    }
}
