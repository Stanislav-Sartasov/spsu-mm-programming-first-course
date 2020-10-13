using FuturePattern.Library;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuturePattern.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestEquals()
        {
            IVectorLengthComputer computerOne = Creator.Create(0); // Cascade
            IVectorLengthComputer computerTwo = Creator.Create(1); // Modified cascade

            int[] data = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int actualOne = computerOne.ComputeLength(data);
            int actualTwo = computerTwo.ComputeLength(data);
            int expected = 16;
            Assert.IsTrue(actualOne == expected && actualTwo == expected);
        }
    }
}
