using Microsoft.VisualStudio.TestTools.UnitTesting;
using CascadeLib;

namespace FifthTask.Tests
{
	[TestClass]
	public class ModelsTests
	{
		private int[] arr;
		private int capacity = 200000;

		private int expected, actual;
		
		[TestInitialize]
		public void TestInit()
		{
			arr = ArrayGenerator.Generate(capacity, 1000);

			var seqModel = new Sequential();
			expected = seqModel.ComputeLength(arr);
		}
		[TestMethod]
		public void CascadeSimpleTest()
		{
			var cascSModel = new CascadeSimple();
			actual = cascSModel.ComputeLength(arr);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CascadeModelTest()
		{
			var cascMModel = new CascadeMod();
			actual = cascMModel.ComputeLength(arr);

			Assert.AreEqual(expected, actual);
		}
	}
}