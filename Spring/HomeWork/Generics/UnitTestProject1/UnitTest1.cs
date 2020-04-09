using Generics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddAndDelete()
        {
            Tree<int> test = new Tree<int>();
            test.add(5, 10);
            test.add(5, 4);
            test.add(5, 15);
            test.add(5, 3);
            test.add(5, 18);
            test.add(5, 12);
            test.add(5, 7);

            Assert.AreEqual(10, test.root.key);
            Assert.AreEqual(5, test.root.value);
            Assert.AreEqual(4, test.root.left.key);
            Assert.AreEqual(3, test.root.left.left.key);
            Assert.AreEqual(7, test.root.left.right.key);

            Assert.AreEqual(15, test.root.right.key);
            Assert.AreEqual(18, test.root.right.right.key);
            Assert.AreEqual(12, test.root.right.left.key);


            test.delete(3);
            Assert.AreEqual(null, test.root.left.left);

            test.delete(7);
            Assert.AreEqual(null, test.root.left.right);

            test.delete(15);
            Assert.AreEqual(null, test.root.right.right);
            Assert.AreEqual(18, test.root.right.key);
            Assert.AreEqual(12, test.root.right.left.key);

        }
    }
}
