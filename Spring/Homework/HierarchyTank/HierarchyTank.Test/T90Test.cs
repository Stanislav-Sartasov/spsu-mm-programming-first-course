using Microsoft.VisualStudio.TestTools.UnitTesting;
using TankConfig;
using System.Collections.Generic;
using System.Linq;

namespace HierarchyTank.Tests
{
    [TestClass]
    public class T90Test
    {
        T90 t90;
        Dictionary<string, int> expIngredients;

        [TestInitialize]
        public void T90Initialize()
        {
            t90 = new T90();
            expIngredients = new Dictionary<string, int> { { "Armor in millimeters", 65 }, { "Weapon caliber in millimeters", 125 }, { "Crew", 3 } };
        }

        [TestMethod]
        public void CorrectName()
        {
            Assert.AreEqual("T-90", t90.Model);
        }

        [TestMethod]
        public void CorrectCountry()
        {
            Assert.AreEqual("Russia", t90.ManufacturerCountry);
        }

        [TestMethod]
        public void CorrectWeight()
        {
            Assert.AreEqual(46, t90.Weight);
        }

        [TestMethod]
        public void CorrectArmament()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(expIngredients, t90.Armament));
        }

        [TestMethod]
        public void CorrectPrintInfo()
        {
            string expStr = "\nModel : T-90\nCountry: Russia\nWeight: 46 tons"
                + $"\nTank Config T-90:\n"
                + "Armor in millimeters: 65\n" + "Weapon caliber in millimeters: 125\n" + "Crew: 3\n";
            Assert.AreEqual(expStr, t90.GetFullInfo());
        }
    }
}