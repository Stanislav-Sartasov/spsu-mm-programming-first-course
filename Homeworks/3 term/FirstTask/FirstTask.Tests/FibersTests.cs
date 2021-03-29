using Microsoft.VisualStudio.TestTools.UnitTesting;
using FibersDescription;
using System.Diagnostics;
using System.Threading;

namespace FirstTask.Tests
{
	[TestClass]
	public class FibersTests
	{
		private int NumOfProcesses { get; set; } = 4;
		
		public void TestsInit()
		{
			for (int i = 0; i < NumOfProcesses; i++)
			{
				ProcessManager.AddProcess();
			}

			Assert.AreEqual(NumOfProcesses, ProcessManager.Fibers.Count);
		}

		[TestMethod]
		public void TestNonPrior()
		{
			ProcessManager.IsPrioritized = false;
			TestsInit();

			ProcessManager.Launch();
			Debug.WriteLine("Launched!");
			Thread.Sleep(1);

			ProcessManager.Dispose();
			Debug.WriteLine("Disposed!");
			Assert.AreEqual(0, ProcessManager.Fibers.Count);
		}
		[TestMethod]
		public void TestPrior()
		{
			ProcessManager.IsPrioritized = true;
			TestsInit();

			ProcessManager.Launch();
			Debug.WriteLine("Launched!");
			Thread.Sleep(1);

			ProcessManager.Dispose();
			Debug.WriteLine("Disposed!");
			Assert.AreEqual(0, ProcessManager.Fibers.Count);
		}
	}
}
