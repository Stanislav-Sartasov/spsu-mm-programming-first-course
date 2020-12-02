using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHierarchy.ClassesHeirs;

namespace SimpleHierarchy.UnitTests
{
    [TestClass]
    public class TestIceCream
    {
        [TestMethod]
        public void BananaIceCreamTest()
        {
            BananaIceCream bananaIceCream = new BananaIceCream();

            string needTo = " Name - Banana Ice Cream\n Weighs - 150\n Gram of milk - 60\n Number of eggs - 2\n Gram of sugar - 10\n Gram of Cream - 30\n Number of bananas - 1\n";

            Assert.AreEqual("Banana Ice Cream", bananaIceCream.IceCreamName);
            Assert.AreEqual(150, bananaIceCream.Weight);
            Assert.AreEqual(60, bananaIceCream.Milk);
            Assert.AreEqual(2, bananaIceCream.Eggs);
            Assert.AreEqual(10, bananaIceCream.Sugar);
            Assert.AreEqual(30, bananaIceCream.Cream);
            Assert.AreEqual(1, bananaIceCream.Bananas);
            Assert.AreEqual(needTo, bananaIceCream.ShowRecipe());
        }

        [TestMethod]
        public void ChocolateIceCreamTest()
        {
            ChocolateIceCream chocolateIceCream = new ChocolateIceCream();

            string needTo = " Name - Chocolate Ice Cream\n Weighs - 900\n Gram of milk - 300\n Number of eggs - 3\n Gram of sugar - 85\n Gram of Cream - 300\n Gram of Chocolate - 100\n";

            Assert.AreEqual("Chocolate Ice Cream", chocolateIceCream.IceCreamName);
            Assert.AreEqual(900, chocolateIceCream.Weight);
            Assert.AreEqual(300, chocolateIceCream.Milk);
            Assert.AreEqual(3, chocolateIceCream.Eggs);
            Assert.AreEqual(85, chocolateIceCream.Sugar);
            Assert.AreEqual(300, chocolateIceCream.Cream);
            Assert.AreEqual(100, chocolateIceCream.Chocolate);
            Assert.AreEqual(needTo, chocolateIceCream.ShowRecipe());
        }
    }
}
