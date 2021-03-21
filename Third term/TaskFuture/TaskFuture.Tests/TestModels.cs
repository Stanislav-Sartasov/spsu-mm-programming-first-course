using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using System;

namespace TaskFuture.Tests
{
    [TestClass]
    public class TestModels
    {
        CascadeModel cascadeModel = new CascadeModel();
        ModifiedCascade modifiedCascade = new ModifiedCascade();
        [TestMethod]
        public void TestCalculation()
        {
            int[] a = new int[] { 10, 20, 30, 40, 50, 1, 2, 3 };
            
            int commonModelAnswer = cascadeModel.ComputeLength(a);
            int modifiedModelAnswer = modifiedCascade.ComputeLength(a);

            int answer = 0;
            foreach (int element in a)
                answer += element * element;
            answer = (int)Math.Sqrt(answer);

            Assert.AreEqual(commonModelAnswer, modifiedModelAnswer);
            Assert.AreEqual(commonModelAnswer, answer);
            
        }
        [TestMethod]
        public void TestVoidVector()
        {
            int[] a = new int[0];
            int commonModelAnswer = cascadeModel.ComputeLength(a);
            int modifiedModelAnswer = modifiedCascade.ComputeLength(a);
            Assert.AreEqual(0, commonModelAnswer);
            Assert.AreEqual(0, modifiedModelAnswer);
        }
    }
}
