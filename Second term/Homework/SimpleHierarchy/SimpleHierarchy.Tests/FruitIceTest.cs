using Microsoft.VisualStudio.TestTools.UnitTesting;
using IceCreamRecipes;
using System.Collections.Generic;
using System.Linq;

namespace SimpleHierarchy.Tests
{
    [TestClass]
    public class FruitIceTest
    {
        FruitIce fruitIce;
        Dictionary<string, int> expIngredients;

        [TestInitialize]
        public void FruitIceInitialize()
        {
            fruitIce = new FruitIce();
            expIngredients = new Dictionary<string, int> { { "Water", 200 }, { "Kiwi", 150 }, { "Sugar", 20 }, { "Lemon juice", 50 } };
        }

        [TestMethod]
        public void CorrectKind()
        {
            Assert.AreEqual("FruitIce", fruitIce.Kind);
        }

        [TestMethod]
        public void CorrectFlavour()
        {
            Assert.AreEqual("Kiwi", fruitIce.Flavour);
        }

        [TestMethod]
        public void CorrectServingWeight()
        {
            Assert.AreEqual(60, fruitIce.ServingWeight);
        }

        [TestMethod]
        public void CorrectIngredients()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(expIngredients, fruitIce.Ingredients));
        }

        [TestMethod]
        public void CorrectPrintInfo()
        {
            string expStr = "\nKind of ice cream: FruitIce\nFlavour: Kiwi\nWeight: 60"
                + $"\nMain ingredients of FruitIce:\n"
                + "Water: 200 grams\n" + "Kiwi: 150 grams\n" + "Sugar: 20 grams\n" + "Lemon juice: 50 grams\n";
            Assert.AreEqual(expStr, fruitIce.GetFullInfo());
        }

    }
}
