using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Future.Tests
{
    [TestClass]
    public class FutureTests
    {
        static int[] vector = ArrayGenerator.GenerateArray(1000000);
        static double expected = Math.Sqrt(vector.Sum(x => x * x));

        static IVectorLengthComputer[] vectorLengthComputers;
        static List<double> results;
            
        [TestInitialize]
        public void Init()
        {
            vectorLengthComputers = new IVectorLengthComputer[] { new CascadeModel(), new ModifiedCascadeModel(), new SequentialModel() };
            results = new List<double>();
            for (int i = 0; i < vectorLengthComputers.Length; i++)
                results.Add(vectorLengthComputers[i].ComputeLength(vector));
        }
        [TestMethod]
        public void CorrectResult()
        {
            Assert.IsTrue(results.All(r => r.Equals(expected)));
        }
    }
}
