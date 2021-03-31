using Microsoft.VisualStudio.TestTools.UnitTesting;
using CascadeLib;

namespace FifthTask.Tests
{
	[TestClass]
	public class FuncTests  //for current capacity
	{
		private int[] a;
		private int capacity = 10;

		[TestInitialize]
		public void TestInit()
		{
			a = new int[capacity];
			for (int i = 0; i < capacity; i++)
			{
				a[i] = i;
			}
		}

		[TestMethod]
		public void SquareTest()
		{
			for (int i = 0; i < capacity; i++)
			{
				Assert.AreEqual(a[i] * a[i], Func.Square(a[i]));
			}
		}

		[TestMethod]
		public void ArrayMathTest()
		{
			Assert.AreEqual(3, (int)Func.Log(capacity));
			Assert.AreEqual(4, Func.NumOfBlocks(capacity));
		}
	}
}