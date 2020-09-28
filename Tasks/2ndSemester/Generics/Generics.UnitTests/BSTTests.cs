using Generics.BST;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Generics.UnitTests
{
    [TestClass]
    public class BSTTests
    {
        [TestMethod]
        public void TreeIntTest()
        {
            Node<int> root = null;
            Tree<int> tree = new Tree<int>();

            root = tree.Insert(root, 1, 1);
            root = tree.Insert(root, 4, 2);
            root = tree.Insert(root, 9, 3);
            root = tree.Insert(root, 16, 4);
            root = tree.Insert(root, 25, 5);

            Assert.AreEqual(1, tree.Search(root, 1).value);
            Assert.AreEqual(4, tree.Search(root, 2).value);
            Assert.AreEqual(9, tree.Search(root, 3).value);
            Assert.AreEqual(16, tree.Search(root, 4).value);
            Assert.AreEqual(25, tree.Search(root, 5).value);

            root = tree.Delete(root, 2);
            root = tree.Delete(root, 3);

            Assert.AreEqual(null, tree.Search(root, 2));
            Assert.AreEqual(null, tree.Search(root, 3));
        }

        [TestMethod]
        public void TreeStringTest()
        {
            Node<string> root = null;
            Tree<string> tree = new Tree<string>();

            root = tree.Insert(root, "a", 1);
            root = tree.Insert(root, "aa", 2);
            root = tree.Insert(root, "aaa", 3);
            root = tree.Insert(root, "aaaa", 4);
            root = tree.Insert(root, "aaaaa", 5);

            Assert.AreEqual("a", tree.Search(root, 1).value);
            Assert.AreEqual("aa", tree.Search(root, 2).value);
            Assert.AreEqual("aaa", tree.Search(root, 3).value);
            Assert.AreEqual("aaaa", tree.Search(root, 4).value);
            Assert.AreEqual("aaaaa", tree.Search(root, 5).value);

            root = tree.Delete(root, 2);
            root = tree.Delete(root, 3);

            Assert.AreEqual(null, tree.Search(root, 2));
            Assert.AreEqual(null, tree.Search(root, 3));
        }
    }
}
