using NUnit.Framework;
using manyTanks;
using abstractTank;

namespace testTank
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void t34Test()
        {
            manyTanks.t34 tank = new manyTanks.t34();
            Assert.Pass();

        }
        [Test]
        public void m4Test()
        {
            manyTanks.m4 tank = new manyTanks.m4();
            Assert.Pass();
        }

    }
}