using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using ThreadPool;

namespace UnitTestThreadPool
{
	[TestClass]
	public class UnitTest
	{
		public static int i = 0;
		static void Hello()
		{
			i++;
		}
		[TestMethod]
		public void TestThreadPool()
		{
			Action a = Hello;
			int n = 3;
			MyThreadPool myPool = new MyThreadPool(n);
			for (int i = 0; i < 5; ++i)
				myPool.Enqueue(a);
			Thread.Sleep(100);
			myPool.Dispose();
			Assert.AreEqual(5, i);
		}
	}
}
