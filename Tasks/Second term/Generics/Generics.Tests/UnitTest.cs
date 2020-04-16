using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicArray;

namespace Generics.Tests
{
    [TestClass]
    public class UnitTest
    { 
        [TestMethod]
        public void TestDynamicArrayCreateForObject()
        {
            try
            {
                DynamicArray<object> dynamicArray = new DynamicArray<object>();
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void TestDynamicArrayAddAndGetAtIdex()
        {
            //arrange
            DynamicArray<int> dynamicArray = new DynamicArray<int>();

            //act
            dynamicArray.Add(2);
            int actual = dynamicArray.GetAtIndex(0);
            int expected = 2;

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestDynamicArrayDeleteByIndex()
        {
            //arrange
            DynamicArray<int> dynamicArray = new DynamicArray<int>();

            //act
            dynamicArray.Add(1);
            dynamicArray.Add(2);
            dynamicArray.Add(3);
            dynamicArray.Delete(1);
            int actual = dynamicArray.GetAtIndex(1);
            int expected = 3;

            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestDynamicArrayFind()
        {
            //arrange
            DynamicArray<int> dynamicArray = new DynamicArray<int>();

            //act
            dynamicArray.Add(1);
            dynamicArray.Add(2);
            dynamicArray.Add(3);
            dynamicArray.Add(4);
            int actual = dynamicArray.Find(3);
            int expected = 2;

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
