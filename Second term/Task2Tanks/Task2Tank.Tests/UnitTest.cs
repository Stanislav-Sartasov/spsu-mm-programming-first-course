using Microsoft.VisualStudio.TestTools.UnitTesting;
using VarietiesOfTanks;

namespace Task2Tank.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void PantherTankTest()
        {
            PantherTank tankSdKfz = new PantherTank("Sd.Kfz. 267", "Medium Tank", "Germany", 44.8F, 6.87F, 2.99F, 5, 720);
            Assert.AreEqual("Sd.Kfz. 267", tankSdKfz.Name);
            Assert.AreEqual("Medium Tank", tankSdKfz.Type);
            Assert.AreEqual("Germany", tankSdKfz.PlaceOfOrigin);
            Assert.AreEqual(44.8F, tankSdKfz.Mass);
            Assert.AreEqual(6.87F, tankSdKfz.Length);
            Assert.AreEqual(2.99F, tankSdKfz.Height);
            Assert.AreEqual(5, tankSdKfz.Crew);
            Assert.AreEqual(720, tankSdKfz.FuelCapacity);
            Assert.AreEqual("Name of the tank: Sd.Kfz. 267\nType: Medium Tank\nPlaceOfOrigin: Germany\nMass: 44,8\n" +
                "Length: 6,87\nHeight: 2,99\nCrew: 5\nFuel capacity: 720", tankSdKfz.GetInfo());
        }

        [TestMethod]
        public void TigerTankTest()
        {
            TigerTank tankPzKpfw = new TigerTank("Pz.Kpfw VI", "Heavy tank", "Germany", 54, 6.316F, 3, 5, 45.4F);
            Assert.AreEqual("Pz.Kpfw VI", tankPzKpfw.Name);
            Assert.AreEqual("Heavy tank", tankPzKpfw.Type);
            Assert.AreEqual("Germany", tankPzKpfw.PlaceOfOrigin);
            Assert.AreEqual(54, tankPzKpfw.Mass);
            Assert.AreEqual(6.316F, tankPzKpfw.Length);
            Assert.AreEqual(3, tankPzKpfw.Height);
            Assert.AreEqual(5, tankPzKpfw.Crew);
            Assert.AreEqual(45.4F, tankPzKpfw.MaxSpeed);
            Assert.AreEqual("Name of the tank: Pz.Kpfw VI\nType: Heavy tank\nPlaceOfOrigin: Germany\nMass: 54\n" +
                "Length: 6,316\nHeight: 3\nCrew: 5\nMaximum speed: 45,4", tankPzKpfw.GetInfo());
        }
    }
}
