using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using TPLib;

namespace FourthTask.Tests
{
	[TestClass]
	public class TPTest
	{
		[TestMethod]
		public void TPTestMethod()
		{
			const int numOfTasks = 20;
			var action = new Action(() => Console.WriteLine("Testing - testing 1, 2, 3!"));

			var tp = new TPLib.ThreadPool();
			Debug.WriteLine("The program has started!");

			for (int i = 0; i < numOfTasks; i++)
			{
				tp.Enqueue(action);
				Thread.Sleep(20);
			}
			Assert.AreEqual(4, tp.ListOfThreads.Count);
			Thread.Sleep(20);

			tp.Dispose();
			Assert.AreEqual(0, tp.ListOfThreads.Count);
			Debug.WriteLine("The program has finished!");
		}
	}
}