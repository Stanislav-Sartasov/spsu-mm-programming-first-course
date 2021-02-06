using Microsoft.VisualStudio.TestTools.UnitTesting;
using FibersDescription;
using System.Diagnostics;

namespace FirstTask.Tests
{
	[TestClass]
	public class FibersTests
	{
		private int NumOfProcesses { get; set; } = 8;
		[TestInitialize]
		public void Init()
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

			ProcessManager.Launch();
			Debug.WriteLine("Launched!");
			//Thread.Sleep(1);

			ProcessManager.Dispose();
			Debug.WriteLine("Disposed!");
			Assert.AreEqual(0, ProcessManager.Fibers.Count);
		}
		[TestMethod]
		public void TestPrior()
		{
			ProcessManager.IsPrioritized = true;

			ProcessManager.Launch();
			Debug.WriteLine("Launched!");
			//Thread.Sleep(1);

			ProcessManager.Dispose();
			Debug.WriteLine("Disposed!");
			Assert.AreEqual(0, ProcessManager.Fibers.Count);
		}
	}
}
