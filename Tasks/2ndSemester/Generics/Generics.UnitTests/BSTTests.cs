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

            root = tree.Insert(root, 1);
            root = tree.Insert(root, 4);
            root = tree.Insert(root, 9);
            root = tree.Insert(root, 16);
            root = tree.Insert(root, 25);

            Assert.AreEqual(1, tree.Search(root, 1).value);
            Assert.AreEqual(4, tree.Search(root, 4).value);
            Assert.AreEqual(9, tree.Search(root, 9).value);
            Assert.AreEqual(16, tree.Search(root, 16).value);
            Assert.AreEqual(25, tree.Search(root, 25).value);

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

            root = tree.Insert(root, "a");
            root = tree.Insert(root, "aa");
            root = tree.Insert(root, "aaa");
            root = tree.Insert(root, "aaaa");
            root = tree.Insert(root, "aaaaa");

            Assert.AreEqual("a", tree.Search(root, "a").value);
            Assert.AreEqual("aa", tree.Search(root, "aa").value);
            Assert.AreEqual("aaa", tree.Search(root, "aaa").value);
            Assert.AreEqual("aaaa", tree.Search(root, "aaaa").value);
            Assert.AreEqual("aaaaa", tree.Search(root, "aaaaa").value);

            root = tree.Delete(root, 2);
            root = tree.Delete(root, 3);

            Assert.AreEqual(null, tree.Search(root, "b"));
            Assert.AreEqual(null, tree.Search(root, "bb"));
        }
    }
}
