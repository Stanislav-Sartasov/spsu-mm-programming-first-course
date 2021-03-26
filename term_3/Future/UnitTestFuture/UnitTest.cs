using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FutureLibrary;

namespace UnitTestFuture
{
    [TestClass]
    public class UnitTest
    {
        int[] testArray = { 3, 4, 5, 6, 7 };
        double testResult;
        [TestInitialize]
        public void Initialize()
        {
            testResult = Math.Sqrt(testArray.Sum((i) => i * i));
        }
        [TestMethod]
        public void TestCascade()
        {
            IVectorLengthComputer vector = new Cascade();
            double result = vector.ComputeLength(testArray);
            Assert.AreEqual(testResult, result);
        }
        [TestMethod]
        public void TestModifiedCascade()
        {
            IVectorLengthComputer vector = new ModifiedCascade();
            double result = vector.ComputeLength(testArray);
            Console.WriteLine($"actual is: {testResult} we have got: {result}");
            Assert.AreEqual(testResult, result);
        }
    }
}
