using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeakReference.Tests
{
    [TestClass]
    public class WeakReferenceTests
    {
        [TestMethod]
        public void TreeMethodsTest()
        {
            Tree<string> test = new Tree<string>(5000);
            test.Add("a", 10);
            test.Add("b", 4);
            test.Add("c", 15);
            test.Add("d", 3);
            test.Add("f", 18);
            test.Add("g", 12);
            test.Add("idk", 7);

            string tmp;
            tmp = test.Find(10);
            test.Find(7);
            test.Find(4);
            test.Find(15);

            Assert.AreEqual(tmp, "a");

            test.Delete(7);
            test.Delete(15);
            test.Delete(4);

            tmp = test.Find(10);
            Assert.AreEqual(tmp, "a");

            tmp = test.Find(7);
            Assert.AreEqual(null, null);
            test.Find(4);
            test.Find(15);
        }
    }
}
