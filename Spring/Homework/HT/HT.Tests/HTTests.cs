using HTClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace HT.Tests
{
	[TestClass]
	public class HTTests
	{
		MyHashtable<int, int> table;
		int[] value;

		[TestMethod]
		public void CorrectAdd()
		{
			table = new MyHashtable<int, int>();
			value = new int[100];

			foreach (int i in value)
			{
				table.MyAdd(i, i);
			}

			foreach (int i in value)
			{
				Assert.IsTrue(table.KeyExistence(i));
				Assert.IsTrue(table.ValueExistence(i));
				table.MySearch(i, out value[i]);
				Assert.AreEqual(i, value[i]);
			}
		}

		[TestMethod]
		public void CorrectDelete()
		{
			table = new MyHashtable<int, int>();
			value = new int[100];

			for (int i = 0; i < 100; i++)
			{
				table.MyAdd(i, i);
			}

			for (int i = 0; i < 100; i++)
			{
				table.MyDelete(i);
			}

			for (int i = 0; i < 100; i++)
			{
				Assert.IsTrue(!table.KeyExistence(i));
				table.MySearch(i, out value[i]);
				Assert.AreEqual(default, value[i]);
			}
		}

		[TestMethod]
		public void CorrectResize()
		{
			table = new MyHashtable<int, int>();
			Assert.AreEqual(table.ListMaxSize, 1);
			Assert.AreEqual(table.ListSize, 4);
			for (int i = 0; i < 8; i++)
				table.MyAdd(i, i);
			Assert.AreEqual(table.ListMaxSize, 2);
			Assert.AreEqual(table.ListSize, 8);
		}
	}
}
