using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tank;

namespace UnitTestTanks
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestM4()
        {
            string TestString = " name: M4 Sharman\n contry: USA\n armor: 51mm\n number of guns: 4 side armor: 38\n tower armor: 76";
            abstractTank first = new m4();
            Assert.AreEqual(TestString, first.getInfo());
        }

        [TestMethod]
        public void TestT34()
        {
            string TestString = " name: T34\n contry: USSR\n armor: 45mm\n number of guns: 3 can go without refuel: 250km";
            abstractTank first = new t34();
            Assert.AreEqual(TestString, first.getInfo());
        }
    }
}
