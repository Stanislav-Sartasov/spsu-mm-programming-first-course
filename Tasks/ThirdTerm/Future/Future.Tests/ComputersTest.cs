using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Future.Library;

namespace Future.Tests
{
    [TestClass]
    public class ComputersTest
    {
        private int SumOfSquaresOfAnArray(int[] a)
        {
            int result = 0;
            for (int i = 0; i < a.Length; i++)
                result += a[i] * a[i];
            return result;
        }

        private int[] ExpextedResults(int[][] datas)
        {
            int[] results = new int[datas.Length];
            for (int i = 0; i < datas.Length; i++)
                results[i] = (int)Math.Sqrt(SumOfSquaresOfAnArray(datas[i]));
            return results;
        }

        [TestMethod]
        public void TestCascade()
        {
            IVectorLengthComputer cascade = new Cascade();

            int[][] datas = new int[][]
            {
                new int[] { -17, -7, 0, 20, 9, -11, 19, 17, -6, -14 },
                new int[] { -9, -3, 7, 8, 13 },
                new int[] { -5, 5, -20, 3, 4, 18, 0, -8, -9, -11, 6, 16, 14, -4, -3 },
                new int[] { 1, 20, -12 },
                new int[] { -14 }
            };

            int[] results = new int[datas.Length];
            int[] expextedResults = ExpextedResults(datas);

            for (int i = 0; i < datas.Length; i++)
                results[i] = cascade.ComputeLength(datas[i]);

            for (int i = 0; i < datas.Length; i++)
                Assert.AreEqual(results[i], expextedResults[i]);
        }

        [TestMethod]
        public void TestRegularAddition()
        {
            IVectorLengthComputer regularAddition = new RegularAddition();

            int[][] datas = new int[][]
            {
                new int[] { -17, -7, 0, 20, 9, -11, 19, 17, -6, -14 },
                new int[] { -9, -3, 7, 8, 13 },
                new int[] { -5, 5, -20, 3, 4, 18, 0, -8, -9, -11, 6, 16, 14, -4, -3 },
                new int[] { 1, 20, -12 },
                new int[] { -14 }
            };

            int[] results = new int[datas.Length];
            int[] expextedResults = ExpextedResults(datas);

            for (int i = 0; i < datas.Length; i++)
                results[i] = regularAddition.ComputeLength(datas[i]);

            for (int i = 0; i < datas.Length; i++)
                Assert.AreEqual(results[i], expextedResults[i]);
        }
    }
}
