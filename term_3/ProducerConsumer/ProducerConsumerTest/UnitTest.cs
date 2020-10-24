using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProducerConsumer;

namespace ProducerConsumerTest
{
	[TestClass]
	public class UnitTest
	{
		[TestMethod]
		public void TestProducerConsumer()
		{
			List<Data<string>> lst = new List<Data<string>>();
			lst.Clear();
			Producer prod = new Producer(lst, "producer 1");
			Thread.Sleep(50);
			//prod.Stop();
			prod.Join();
			Assert.AreEqual(1, lst.Count);

			Consumer cons = new Consumer(lst, "consumer 1");
			Thread.Sleep(50);
			//cons.Stop();
			cons.Join();
			Assert.AreEqual(0, lst.Count);
		}
	}
}
