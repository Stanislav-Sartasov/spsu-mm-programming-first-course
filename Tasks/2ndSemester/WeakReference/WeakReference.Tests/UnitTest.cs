using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using WeakReference.WeakBinaryTree;

namespace WeakReference.Tests
{
    [TestClass]
    public class UnitTest
    {
        public void TestTree(WeakBinaryTree<string> weakBinaryTree)
        {
            var obj = new String("1");
            weakBinaryTree.Add(obj);
        }

        [TestMethod]
        public void TestAdd()
        {
            WeakBinaryTree<string> weakBinaryTree = new WeakBinaryTree<string>(5000);
            TestTree(weakBinaryTree);

            Assert.AreEqual("1", weakBinaryTree.GetValue(weakBinaryTree.Root));
        }

        [TestMethod]
        public void TestRemove()
        {
            WeakBinaryTree<string> weakBinaryTree = new WeakBinaryTree<string>(5000);
            TestTree(weakBinaryTree);

            weakBinaryTree.Remove("1");

            Assert.IsTrue(null == weakBinaryTree.GetValue(weakBinaryTree.Root));
        }

        [TestMethod]
        public void TestFind()
        {
            WeakBinaryTree<string> weakBinaryTree = new WeakBinaryTree<string>(5000);
            TestTree(weakBinaryTree);

            Assert.IsTrue(null != weakBinaryTree.Find("1"));
        }

        [TestMethod]
        public void TestLifeTime()
        {
            WeakBinaryTree<string> weakBinaryTree = new WeakBinaryTree<string>(1000);
            TestTree(weakBinaryTree);

            Thread.Sleep(1500);
            GC.Collect();

            Assert.AreEqual(null, weakBinaryTree.GetValue(weakBinaryTree.Find("1")));
        }
    }
}
