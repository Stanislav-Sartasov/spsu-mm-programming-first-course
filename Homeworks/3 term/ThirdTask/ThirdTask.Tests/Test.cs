using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace ThirdTask.Tests
{
	[TestClass]
	public class Test
	{
		private const int producersNum = 3;
		private const int consumersNum = 3;

		[TestMethod]
		public void TestMethod()
		{
			var manager = new Manager(producersNum, consumersNum);

			manager.Start(0);
			Thread.Sleep(50);
			manager.Finish(0);

			Assert.AreNotEqual(0, manager.Data.Count);

			manager.Start(1);
			Thread.Sleep(50);
			manager.Finish(1);

			Assert.AreEqual(0, manager.Data.Count);
		}

	}
}