using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecondTask.Tests
{
	[TestClass]
	public class SortTestSmall
	{
		public List<int> arr;
		public int capacity = 32;

		[TestInitialize]
		public void Init()
		{
			arr = new();

			for (int i = 0; i < capacity; i++)
			{
				arr.Add(capacity - 1 - i);
			}
		}

		[TestMethod]
		public void SmallArrayTestMethod()
		{
			for (int i = 0; i < capacity; i++)
			{
				Assert.AreNotEqual(i, arr[i]);
			}

			SingleQuickSort.QuickSort(arr, 0, arr.Count - 1);

			for (int i = 0; i < capacity; i++)
			{
				Assert.AreEqual(i, arr[i]);
			}
		}
	}
}
