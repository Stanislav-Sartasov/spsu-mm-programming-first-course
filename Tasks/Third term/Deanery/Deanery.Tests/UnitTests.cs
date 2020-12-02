using Deanery.System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MutexLockList;

using SpinLockList;

namespace Deanery.Tests
{
    [TestClass]
    public class UnitTests
    {
        // I suspect that only one mutex is being created for all lists

        // Result:
        // Idk how, but it looks like the truth, naming each mutex solved the speed problem
        [TestMethod]
        public void TestMutexs()
        {
            MyMutexList<int>[] table = new MyMutexList<int>[2] { new MyMutexList<int>(), new MyMutexList<int>() };

            Assert.AreNotEqual(table[0].GetMutex(), table[1].GetMutex());
        }

        // too long test, cause of size is 9999
        //[TestMethod]
        //public void TestMutexTable()
        //{
        //    IHashTable mySystem = new MutexExamSystem();
        //    MyMutexList<(long, long)>[] table = (MyMutexList<(long, long)>[])mySystem.GetTable();

        //    for (int i = 0; i < table.Length; i++)
        //    {
        //        for (int j = i + 1; j < table.Length; j++)
        //            Assert.AreNotEqual(table[i].GetMutex(), table[j].GetMutex());
        //    }
        //}

        [TestMethod]
        public void TestMySpinLockList()
        {
            MySpinLockList<int> myList = new MySpinLockList<int>(-1);
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
            IExamSystem mySystem = new ListExamSystem(9999);
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
            IExamSystem mySystem = new MutexExamSystem(9999);
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
