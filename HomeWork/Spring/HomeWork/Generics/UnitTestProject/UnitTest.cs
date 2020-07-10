using Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestTreeMethods()
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
