using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FutureLibrary;

namespace UnitTestFuture
{
    [TestClass]
    public class UnitTest
    {
        int[] testArray = { 3, 4 };
        double TestResult;
        [TestInitialize]
        public void Initialize()
        {
            TestResult = Math.Sqrt(testArray.Sum((i) => i * i));
        }
        [TestMethod]
        public void TestCascade()
        {
            IVectorLengthComputer vector = new Cascade();
            double result = vector.ComputeLength(testArray);
            Assert.AreEqual(TestResult, result);
        }
    }
}
