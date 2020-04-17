using Hierarchy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HierarchyTest
{
    [TestClass]
    public class HierarchyTest
    {
        [TestMethod]
        public void T34Test()
        {
            T34 t34Test = new T34();
            Assert.AreEqual("Black", t34Test.Color);
            Assert.AreEqual("Ussr", t34Test.CountryOfManufacture);
            Assert.AreEqual(56, t34Test.MaxSpeed);
            Assert.AreEqual(400, t34Test.Armor);
            Assert.AreEqual(4, t34Test.CabinCrew);

        }

        [TestMethod]
        public void T49Test()
        {
            T49 t49Test = new T49();
            Assert.AreEqual("Brown", t49Test.Color);
            Assert.AreEqual("the USA", t49Test.CountryOfManufacture);
            Assert.AreEqual(65, t49Test.MaxSpeed);
            Assert.AreEqual(1300, t49Test.Armor);
            Assert.AreEqual(4, t49Test.CabinCrew);

        }

        [TestMethod]
        public void PanterTest()
        {
            Panter PanterTest = new Panter();
            Assert.AreEqual("Grey", PanterTest.Color);
            Assert.AreEqual("German", PanterTest.CountryOfManufacture);
            Assert.AreEqual(55, PanterTest.MaxSpeed);
            Assert.AreEqual(1200, PanterTest.Armor);
            Assert.AreEqual(5, PanterTest.CabinCrew);

        }

        [TestMethod]
        public void PrintTest()
        {
            Panter panter = new Panter();
            panter.Print();
            panter.Print("Color");
            panter.Print("Country Of Manufacture");
            panter.Print("Maximum Speed");
            panter.Print("Armor");
            panter.Print("Cabin Crew");
        }
    }
}
