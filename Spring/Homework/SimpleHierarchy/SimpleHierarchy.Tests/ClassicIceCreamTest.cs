using Microsoft.VisualStudio.TestTools.UnitTesting;
using IceCreamRecipes;
using System.Collections.Generic;
using System.Linq;

namespace SimpleHierarchy.Tests
{
    [TestClass]
    public class ClassicIceCreamTest
    {
        ClassicIceCream classicIC;
        Dictionary<string, int> expIngredients;

        [TestInitialize]
        public void ClassicIceCreamInitialize()
        {
            classicIC = new ClassicIceCream();
            expIngredients = new Dictionary<string, int> { { "Cream", 300 }, { "Milk", 200 }, { "Egg yolk", 40 }, { "Powdered sugar", 25 }, { "Vanilla", 25 } };
        }

        [TestMethod]
        public void CorrectKind()
        {
            Assert.AreEqual("Milk ice cream", classicIC.Kind);
        }

        [TestMethod]
        public void CorrectFlavour()
        {
            Assert.AreEqual("Vanilla", classicIC.Flavour);
        }

        [TestMethod]
        public void CorrectServingWeight()
        {
            Assert.AreEqual(80, classicIC.ServingWeight);
        }

        [TestMethod]
        public void CorrectIngredients()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(expIngredients, classicIC.Ingredients));
        }

        [TestMethod]
        public void CorrectPrintInfo()
        {
            string expStr = "\nKind of ice cream: Milk ice cream\nFlavour: Vanilla\nWeight: 80"
                + $"\nMain ingredients of Milk ice cream:\n"
                + "Cream: 300 grams\n" + "Milk: 200 grams\n" + "Egg yolk: 40 grams\n" + "Powdered sugar: 25 grams\n" + "Vanilla: 25 grams\n";
            Assert.AreEqual(expStr, classicIC.GetFullInfo());
        }
    }
}
