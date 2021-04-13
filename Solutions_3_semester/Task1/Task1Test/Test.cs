using NUnit.Framework;
using Processes;

namespace Task1Test
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Test()
		{
			Assert.IsTrue(ProcessManager.Init(new Process[] { new Process() }));
			ProcessManager.Dispose();
		}
	}
}