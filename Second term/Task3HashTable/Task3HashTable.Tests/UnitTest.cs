using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task3HashTable;

namespace Task3HashTable.Tests
{
    [TestClass]
    public class UnitTest
    {
        Map<string, double> map = new Map<string, double>();
        private bool IsNodeExist(string key, double value)
        {
            int bucketIndex = map.GetBucket(key);
            var head = map.bucketList[bucketIndex].First;

            while (head != null)
            {
                if (head.Value.Key.Equals(key) && head.Value.Value.Equals(value))
                    return true;

                head = head.Next;
            }
            return false;
        }

        [TestMethod]
        public void TestMapMethods()
        {

            
            for (int i = 0; i < 2000; i++)
            {
                map.Add($"Value {i}", i);
            }

            for (int i = 0; i < 2000; i++)
            {
                Assert.IsTrue(IsNodeExist($"Value {i}", i)); // Тест методов Add, Search и Resize
                map.Delete($"Value {i}");
                Assert.IsTrue(!(IsNodeExist($"Value {i}", i))); // Тест метода Delete
            }
        }
        
    }
}
