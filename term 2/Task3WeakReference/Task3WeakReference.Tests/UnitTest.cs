using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task3WeakReference.Tests
{
    [TestClass]
    public class UnitTest
    {
        WeakHashMap<int, string> testWeakHashMap = new WeakHashMap<int, string>(1000);
        private bool IsNodeExist(int key, string value)
        {
            int bucketIndex = testWeakHashMap.GetBucket(key);
            var head = testWeakHashMap.bucketList[bucketIndex].First;

            while (head != null)
            {
                if (head.Value.Node.TryGetTarget(out HashNode<int, string> target) && target.Key.Equals(key)
                    && target.Value.Equals(value))
                {
                    return true;
                }
                head = head.Next;
            }
            return false;
        }
        [TestMethod]
        public void TestLifeTime()
        {

            for (int i = 0; i < 2000; i++)
            {
                testWeakHashMap.Add(i, $"Value {i}");
            }
            Thread.Sleep(2000);
            GC.Collect();
            for (int i = 0; i < 2000; i++)
            {
                Assert.AreEqual(false, testWeakHashMap.Search(i));
            }
        }

        [TestMethod]
        public void TestMapMethods()
        {
            for (int i = 0; i < 2000; i++)
            {
                testWeakHashMap.Add(i, $"Value {i}");
            }
            
            for (int i = 0; i < 2000; i++)
            {
                Assert.IsTrue(IsNodeExist(i, $"Value {i}"));
                testWeakHashMap.Delete(i);
                Assert.IsTrue(!(IsNodeExist(i, $"Value {i}")));
            }
        }
    }
    
}
