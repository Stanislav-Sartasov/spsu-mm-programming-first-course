using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HashTableLib.Tests
{
    [TestClass]
    public class HashTableTest
    {
        Hashtable<double, string> table = new Hashtable<double, string>(5000);
        string[] values = new string[100];

        [TestMethod]
        public void CorrectAdd()
        {
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
        public void CorrectWeakReferenceCollect()
        {
            for (int i = 0; i < 100; i++)
            {
                table.AddPair(i, $"{i}");
            }
            Assert.AreEqual(100, table.CountPairs());
            Thread.Sleep(6000);
            GC.Collect();
            Assert.AreEqual(0, table.CountPairs(), 5);
            table.Clear();
            for (int i = 0; i < 100; i++)
            {
                table.AddPair(i, $"{i}");
                if (i < 50)
                    table.TryGetValue(i,out values[i]);
            }
            Thread.Sleep(6000);
            GC.Collect();
            Assert.AreEqual(50, table.CountPairs(), 5);
        }
    }
}
