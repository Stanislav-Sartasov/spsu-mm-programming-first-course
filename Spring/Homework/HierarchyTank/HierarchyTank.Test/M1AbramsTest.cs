using Microsoft.VisualStudio.TestTools.UnitTesting;
using TankConfig;
using System.Collections.Generic;
using System.Linq;

namespace HierarchyTank.Tests
{
    [TestClass]
    public class M1AbramsTest
    {
        M1Abrams m1Abrams;
        Dictionary<string, int> expArmament;

        [TestInitialize]
        public void M1AbramsInitialize()
        {
            m1Abrams = new M1Abrams();
            expArmament = new Dictionary<string, int> { { "Armor in millimeters", 50 }, { "Weapon caliber in millimeters", 120 }, { "Crew", 4 } };
        }

        [TestMethod]
        public void CorrectName()
        {
            Assert.AreEqual("M1-Abrams", m1Abrams.Model);
        }

        [TestMethod]
        public void CorrectCountry()
        {
            Assert.AreEqual("USA", m1Abrams.ManufacturerCountry);
        }

        [TestMethod]
        public void CorrectWeight()
        {
            Assert.AreEqual(55, m1Abrams.Weight);
        }

        [TestMethod]
        public void CorrectArmament()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(expArmament, m1Abrams.Armament));
        }

        [TestMethod]
        public void CorrectPrintInfo()
        {
            string expStr = "\nModel : M1-Abrams\nCountry: USA\nWeight: 55 tons"
                + $"\nTank Config M1-Abrams:\n"
                + "Armor in millimeters: 50\n" + "Weapon caliber in millimeters: 120\n" + "Crew: 4\n";
            Assert.AreEqual(expStr, m1Abrams.GetFullInfo());
        }
    }
}