using Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericsTest
{
    [TestClass]
    public class GenericsTest
    {
        [TestMethod]
        public void TreeMethodsTest()
        {
            Tree<int> Test = new Tree<int>();
            Test.Add(5, 10);
            Test.Add(5, 4);
            Test.Add(5, 15);
            Test.Add(5, 3);
            Test.Add(5, 18);
            Test.Add(5, 12);
            Test.Add(5, 7);

            Test.Delete(3);

            Test.Delete(7);

            Test.Delete(15);


            int num;
            num = Test.Find(12);
            Assert.AreEqual(5, num);
            num = Test.Find(10);
            Assert.AreEqual(5, num);

        }
    }
}