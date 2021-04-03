using NUnit.Framework;
using System;
using System.Threading;

namespace Task4Test
{
	public class Tests
	{
		int counter = 0;
		[Test]
		public void Test()
		{
			Task4.ThreadPool pool = new Task4.ThreadPool();
			for (int i = 0; i < 25; i++)
				pool.Enqueue(Action);

			Thread.Sleep(100);
			pool.Dispose();

			Assert.AreEqual(25, counter);
			Console.WriteLine("ThreadPool log:");
			foreach (var s in pool.Log)
				Console.WriteLine(s);
		}

		void Action()
		{
			Interlocked.Increment(ref counter);
		}
	}
}