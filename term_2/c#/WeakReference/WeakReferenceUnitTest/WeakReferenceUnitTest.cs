using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryWeakRef;

namespace WeakReferenceUnitTest
{
    [TestClass]
    public class WeakReferenceUnitTest
    {
        [TestMethod]
        public void TestWeakCleanTimeClean()
        {
            TimeSpan interval = new TimeSpan(0, 0, 0);
            JenericTree<int> t = new JenericTree<int>(interval);
            int j = 0;
            int d = 4;
            bool weak = false;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);

            t.WeakDelete(d);
            
            for (int i = 0; i < 10; ++i)
            {
                j = -1;
                if (i == d)
                 Assert.IsFalse(t.WeakFind(d.GetHashCode(), ref j, ref weak));
                else
                {
                    Assert.IsTrue(t.WeakFind(i.GetHashCode(), ref j, ref weak));
                }
            }
        }
        [TestMethod]
        public void TestWeakCleanTimeToSave()
        {
            TimeSpan interval = new TimeSpan(0, 0, 20);
            JenericTree<int> t = new JenericTree<int>(interval);
            int j = 0;
            int d = 4;
            bool weak = false;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);

            t.WeakDelete(d);

            for (int i = 0; i < 10; ++i)
            {
                j = -1;
                if (i == d)
                {
                    Assert.IsTrue(t.WeakFind(d.GetHashCode(), ref j, ref weak));
                    Assert.AreEqual(true, weak);
                }
                else
                {
                    Assert.IsTrue(t.WeakFind(i.GetHashCode(), ref j, ref weak));
                }
            }
        }
        [TestMethod]
        public void TestWeakCleanTimeToSaveButCollect()
        {
            TimeSpan interval = new TimeSpan(0, 0, 20);
            JenericTree<int> t = new JenericTree<int>(interval);
            int j = 0;
            int d = 4;
            bool weak = false;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);

            t.WeakDelete(d);
            GC.Collect();

            for (int i = 0; i < 10; ++i)
            {
                j = -1;
                if (i == d)
                {
                    Assert.IsFalse(t.WeakFind(d.GetHashCode(), ref j, ref weak));
                    Assert.AreEqual(false, weak);
                }
                else
                {
                    Assert.IsTrue(t.WeakFind(i.GetHashCode(), ref j, ref weak));
                }
            }
        }
    }
}
