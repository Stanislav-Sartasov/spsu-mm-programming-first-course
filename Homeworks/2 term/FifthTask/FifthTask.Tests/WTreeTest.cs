using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System;
using FifthTask;
using FifthTask.WTreeDescription;

namespace FifthTask.Tests
{
	[TestClass]
	public class WTreeTest
	{
		[TestMethod]
		public void WTreeTestMethod()
		{
			var testTree = new WTree<MyTestClass>(time: 1200);

			testTree.AddElement(4, new MyTestClass(50));
			testTree.AddElement(2, new MyTestClass(20));
			testTree.AddElement(1, new MyTestClass(30));
			testTree.AddElement(3, new MyTestClass(45));
			testTree.AddElement(8, new MyTestClass(34));

			Assert.AreEqual(50, testTree.SearchElement(4).GetData().Num);
			Assert.AreEqual(20, testTree.SearchElement(2).GetData().Num);
			Assert.AreEqual(30, testTree.SearchElement(1).GetData().Num);
			Assert.AreEqual(45, testTree.SearchElement(3).GetData().Num);
			Assert.AreEqual(34, testTree.SearchElement(8).GetData().Num);

			testTree.AddElement(2, new MyTestClass(25));
			testTree.AddElement(6, new MyTestClass(62));
			testTree.AddElement(7, new MyTestClass(48));
			testTree.AddElement(5, new MyTestClass(42));
			testTree.AddElement(12, new MyTestClass(40));

			Assert.AreEqual(25, testTree.SearchElement(2).GetData().Num);
			Assert.AreEqual(62, testTree.SearchElement(6).GetData().Num);
			Assert.AreEqual(48, testTree.SearchElement(7).GetData().Num);
			Assert.AreEqual(42, testTree.SearchElement(5).GetData().Num);
			Assert.AreEqual(40, testTree.SearchElement(12).GetData().Num);

			Assert.AreEqual(null, testTree.SearchElement(10));


			testTree.RemoveElement(8);
			Assert.AreEqual(null, testTree.SearchElement(8));

			testTree.RemoveElement(4);
			Assert.AreEqual(null, testTree.SearchElement(4));

			Thread.Sleep(3000);
			GC.Collect();
			Assert.AreEqual(null, testTree.SearchElement(12));
		}
	}
}