using DynamicArray;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace WeakReference.Tests
{
    [TestClass]
    public class TestWeakArray
    {
        private void AddToArray(WeakDynamicArray<String> dynamicArray)
        {
            var o1 = new String("1");
            var o2 = new String("2");
            var o3 = new String("3");

            dynamicArray.AddToEnd(o1);
            dynamicArray.AddToEnd(o2);
            dynamicArray.AddToEnd(o3);
        }

        private void SetAtIndexToArray(WeakDynamicArray<String> dynamicArray)
        {
            var o1 = new String("1");
            var o2 = new String("2");
            var o3 = new String("3");

            dynamicArray.SetAtIndex(o1, 0);
            dynamicArray.SetAtIndex(o2, 1);
            dynamicArray.SetAtIndex(o3, 2);
        }

        [TestMethod]
        public void TestAddToEndAndGetByIndex()
        {
            WeakDynamicArray<String> dynamicArray = new WeakDynamicArray<String>(5000);

            AddToArray(dynamicArray);
            string actual = dynamicArray.GetByIndex(0);

            Assert.AreEqual("1", actual);
        }

        [TestMethod]
        public void TestSetAtIndexAndGetByIndex()
        {
            WeakDynamicArray<String> dynamicArray = new WeakDynamicArray<String>(5000);

            SetAtIndexToArray(dynamicArray);
            string actual = dynamicArray.GetByIndex(1);

            Assert.AreEqual("2", actual);
        }

        [TestMethod]
        public void TestFind()
        {
            WeakDynamicArray<String> dynamicArray = new WeakDynamicArray<String>(5000);

            AddToArray(dynamicArray);
            int actual = dynamicArray.Find("1");

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void TestLifetime()
        {
            WeakDynamicArray<String> dynamicArray = new WeakDynamicArray<String>(5000);

            AddToArray(dynamicArray);
            Thread.Sleep(6000);
            GC.Collect();
            int actual = dynamicArray.Find("1");

            Assert.AreEqual(-1, actual);
        }
    }
}
