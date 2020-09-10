using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using TreeDescription;

namespace FourthTask.Tests
{
        [TestClass]
        public class TreeTests
        {
                [TestMethod]
                public void IntTest()
                {
                        var treeInt = new BinaryTree<int>();

                        treeInt.AddElement(15, 30);
                        treeInt.AddElement(10, 21);
                        treeInt.AddElement(21, 42);
                        treeInt.AddElement(20, 40);
                        treeInt.AddElement(25, 150);
                        treeInt.AddElement(40, 80);

                        Assert.AreEqual(30, treeInt.SearchElement(15).Data);
                        Assert.AreEqual(21, treeInt.SearchElement(10).Data);
                        Assert.AreEqual(42, treeInt.SearchElement(21).Data);
                        Assert.AreEqual(40, treeInt.SearchElement(20).Data);
                        Assert.AreEqual(150, treeInt.SearchElement(25).Data);
                        Assert.AreEqual(80, treeInt.SearchElement(40).Data);
                        Assert.AreEqual(null, treeInt.SearchElement(45));

                        treeInt.RemoveElement(25);
                        treeInt.RemoveElement(15);

                        Assert.AreEqual(null, treeInt.SearchElement(15));
                        Assert.AreEqual(null, treeInt.SearchElement(25));
                }
                [TestMethod]
                public void StringTest()
                {
                        var treeString = new BinaryTree<string>();

                        treeString.AddElement(2, "mother");
                        treeString.AddElement(3, "father");
                        treeString.AddElement(5, "their son");

                        Assert.AreEqual("mother", treeString.SearchElement(2).Data);
                        Assert.AreEqual("their son", treeString.SearchElement(5).Data);
                        Assert.AreEqual(null, treeString.SearchElement(4));

                        treeString.RemoveElement(2);
                        treeString.RemoveElement(1);

                        Assert.AreEqual(null, treeString.SearchElement(2));
                }
        }
}
