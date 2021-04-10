using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeansOffice;
using DeansOffice.ExamSystems;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestTTASListDeanery()
        {
            IExamSystem examSystem = new TTASListDeanery(9999);

            examSystem.Add(1, 1);
            examSystem.Add(2, 2);
            examSystem.Add(3, 1);

            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsTrue(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));
            Assert.IsFalse(examSystem.Contains(3, 3));

            examSystem.Remove(1, 3);
            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsTrue(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));

            examSystem.Remove(2, 2);
            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsFalse(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));

            examSystem.Remove(2, 2);
            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsFalse(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));

            examSystem.Remove(1, 1);
            examSystem.Remove(3, 1);
            Assert.IsFalse(examSystem.Contains(1, 1));
            Assert.IsFalse(examSystem.Contains(2, 2));
            Assert.IsFalse(examSystem.Contains(3, 1));
        }

        [TestMethod]
        public void TestTTASDeanery()
        {
            IExamSystem examSystem = new TTASDeanery();

            examSystem.Add(1, 1);
            examSystem.Add(2, 2);
            examSystem.Add(3, 1);

            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsTrue(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));
            Assert.IsFalse(examSystem.Contains(3, 3));

            examSystem.Remove(1, 3);
            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsTrue(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));

            examSystem.Remove(2, 2);
            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsFalse(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));

            examSystem.Remove(2, 2);
            Assert.IsTrue(examSystem.Contains(1, 1));
            Assert.IsFalse(examSystem.Contains(2, 2));
            Assert.IsTrue(examSystem.Contains(3, 1));

            examSystem.Remove(1, 1);
            examSystem.Remove(3, 1);
            Assert.IsFalse(examSystem.Contains(1, 1));
            Assert.IsFalse(examSystem.Contains(2, 2));
            Assert.IsFalse(examSystem.Contains(3, 1));
        }
    }   
}
