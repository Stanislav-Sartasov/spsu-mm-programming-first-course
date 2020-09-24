using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using WRMyHashTableClass;

namespace HashTableLib.Tests
{
	[TestClass]
	public class HashTableTests
	{
		MyHashtable<int, string> table;
		string[] values = new string[100];

		[TestMethod]
		public void CorrectAdd()
		{
			table = new MyHashtable<int, string>(5000);
			for (int i = 0; i < 100; i++)
			{
				table.MyAdd(i, $"{i}");
			}

			for (int i = 0; i < 100; i++)
			{
				Assert.IsTrue(table.KeyExistence(i));
				Assert.IsTrue(table.ValueExistence($"{i}"));
				table.MySearch(i, out values[i]);
				Assert.AreEqual($"{i}", values[i]);
			}
		}

		[TestMethod]
		public void CorrectDelete()
		{
			table = new MyHashtable<int, string>(5000);
			for (int i = 0; i < 100; i++)
			{
				table.MyAdd(i, $"{i}");
			}

			for (int i = 0; i < 100; i++)
			{
				table.MyDelete(i);
			}

			for (int i = 0; i < 100; i++)
			{
				Assert.IsTrue(!table.ValueExistence($"{i}"));
				Assert.IsTrue(!table.KeyExistence(i));
				table.MySearch(i, out values[i]);
				Assert.AreEqual(default, values[i]);
			}
		}


		[TestMethod]
		public void CorrectWeakReferenceCollect()
		{
			table = new MyHashtable<int, string>(5000);
			for (int i = 0; i < 100; i++)
			{
				table.MyAdd(i, $"{i}");
			}
			Assert.AreEqual(100, table.NumOfPairs());
			Thread.Sleep(6000);
			GC.Collect();
			Assert.AreEqual(0, table.NumOfPairs(), 5);
			table = new MyHashtable<int, string>(5000);
			for (int i = 0; i < 100; i++)
			{
				table.MyAdd(i, $"{i}");
				if (i < 50)
					table.MySearch(i, out values[i]);
			}
			Thread.Sleep(6000);
			GC.Collect();
			Assert.AreEqual(50, table.NumOfPairs(), 5);
		}
	}
}