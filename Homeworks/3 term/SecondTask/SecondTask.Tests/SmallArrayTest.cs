using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecondTask.Tests
{
	[TestClass]
	public class SmallArrayTest
	{
		public int[] arr;
		public int capacity = 16;

		[TestInitialize]
		public void Init()
		{
			arr = new int[capacity];

			for (int i = 0; i < 16; i++)
			{
				arr[i] = 15 - i;
			}
		}
		[TestMethod]
		public void SmallArrayTestMethod()
		{
			for (int i = 0; i < 16; i++)
			{
				Assert.AreNotEqual(i, arr[i]);
			}
		}
	}
}
