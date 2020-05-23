using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryJeneric;

namespace JenericUnitTest
{
    [TestClass]
    public class JenericTest
    {
        [TestMethod]
        public void TestIntInsert()
        {
            JenericTree<int> t = new JenericTree<int>();
            int j = 0;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);
            for (int i = 0; i < 10; ++i)
            {
                j = -1;
                Assert.IsTrue(t.Find(i.GetHashCode(), ref j));
                Assert.AreEqual(i, j);
            }
            // Assert.IsTrue(t.Find(100.GetHashCode(), ref j));
        }

        [TestMethod]
        public void TestIntDeleteFirstPiece()
        {
            JenericTree<int> t = new JenericTree<int>();
            int j = -1;
            int d = 1;
            for (int i = 0; i < 2; ++i)
                t.Insert(i);

            t.Delete(d);

            Assert.IsFalse(t.Find(d.GetHashCode(), ref j));
            Assert.IsTrue(t.Find(0.GetHashCode(), ref j));
            Assert.AreEqual(0, j);
        }

        [TestMethod]
        public void TestIntDeleteHead()
        {
            JenericTree<int> t = new JenericTree<int>();
            int j = 0;
            int d = 0;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);

            t.Delete(d);
            for (int i = 0; i < 10; ++i)
            {
                if (i == d)
                    Assert.IsFalse(t.Find(d.GetHashCode(), ref j));
                else
                    Assert.IsTrue(t.Find(i.GetHashCode(), ref j));
            }
        }
        [TestMethod]
        public void TestIntDeleteFarePiece()
        {
            JenericTree<int> t = new JenericTree<int>();
            int j = 0;
            int d = 4;
            for (int i = 0; i < 10; ++i)
                t.Insert(i);

            t.Delete(d);
            for (int i = 0; i < 10; ++i)
            {
                j = -1;
                if (i == d)
                    Assert.IsFalse(t.Find(d.GetHashCode(), ref j));
                else
                {
                    Assert.IsTrue(t.Find(i.GetHashCode(), ref j));
                }
            }
        }
        [TestMethod]
        public void TestIsInTree()
        {
            JenericTree<int> t = new JenericTree<int>();
            for (int i = 0; i < 10; ++i)
                t.Insert(i);


            for (int i = 0; i < 10; ++i)
            {
                Assert.IsTrue(t.IsInTree(i));
            }
            Assert.IsFalse(t.IsInTree(11));
        }
    }
}
