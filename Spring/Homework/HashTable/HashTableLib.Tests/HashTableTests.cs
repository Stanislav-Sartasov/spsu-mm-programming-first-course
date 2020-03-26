using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HashTableLib.Tests
{
    [TestClass]
    public class HashTableTest
    {
        Hashtable<double, string> table = new Hashtable<double, string>();

        [TestMethod]
        public void CorrectAdd()
        {
            string[] values = new string[100];
            for (int i = 0; i < 100; i++)
            {
                table.AddPair(i, $"{i}");
            }

            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(table.ContainsValue($"{i}"));
                Assert.IsTrue(table.ContainsKey(i));
                table.TryGetValue(i, out values[i]);
                Assert.AreEqual($"{i}", values[i]);
            }
        }

        [TestMethod]
        public void CorrectDelete()
        {
            string[] values = new string[100];
            for (int i = 0; i < 100; i++)
            {
                table.AddPair(i, $"{i}");
            }

            for (int i = 0; i < 100; i++)
            {
                table.DeleteByKey(i);
            }

            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(!table.ContainsValue($"{i}"));
                Assert.IsTrue(!table.ContainsKey(i));
                table.TryGetValue(i, out values[i]);
                Assert.AreEqual(default, values[i]);
            }
        }

        [TestMethod]
        public void CorrectResize()
        {
            table.AddPair(Math.PI, "PI");
            table.AddPair(0.71321, "0.71321");
            table.AddPair(0.71322, "0.71322");
            table.AddPair(0.71323, "0.71322");
            Assert.AreEqual(2, table.MaxLenOfList);
            Assert.AreEqual(8, table.NumOfLists);
            for (int i = 0; i < 100; i++)
            {
                table.AddPair(i, $"{i}");
            }
            Assert.AreEqual(104, table.Count);
            Assert.AreEqual(table.MaxLenOfList, table.NumOfLists / 4);
        }
    }
}
