using Microsoft.VisualStudio.TestTools.UnitTesting;
using IceCreamRecipes;
using System.Collections.Generic;
using System.Linq;


namespace SimpleHierarchy.Tests
{
    [TestClass]
    public class SundaeTest
    {
        public Sundae sundaeWithTopping;
        public Sundae sundaeWithoutTopping;
        public string topping;
        Dictionary<string, int> expIngredients;
        Dictionary<string, int> expIngredientsTopping;

        [TestInitialize]
        public void SundaeInitialize()
        {
            topping = "Chocolate";
            sundaeWithTopping = new Sundae(topping);
            sundaeWithoutTopping = new Sundae();
            expIngredients = new Dictionary<string, int> { { "Milk", 300 }, { "Strawberry", 200 }, { "Cream", 150 }, { "Sugar", 70 } };
            expIngredientsTopping = new Dictionary<string, int> { { "Milk", 300 }, { "Strawberry", 200 }, { "Cream", 150 }, { "Sugar", 70 }, { "Chocolate", 20 } };
        }

        [TestMethod]
        public void CorrectKind()
        {
            Assert.AreEqual("Sundae", sundaeWithTopping.Kind);
            Assert.AreEqual("Sundae", sundaeWithoutTopping.Kind);
        }

        [TestMethod]
        public void CorrectFlavour()
        {
            Assert.AreEqual("Strawberry", sundaeWithTopping.Flavour);
            Assert.AreEqual("Strawberry", sundaeWithoutTopping.Flavour);
        }

        [TestMethod]
        public void CorrectServingWeight()
        {
            Assert.AreEqual(130, sundaeWithTopping.ServingWeight);
            Assert.AreEqual(120, sundaeWithoutTopping.ServingWeight);
        }

        [TestMethod]
        public void CorrectIngredients()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(expIngredientsTopping, sundaeWithTopping.Ingredients));
            Assert.IsTrue(Enumerable.SequenceEqual(expIngredients, sundaeWithoutTopping.Ingredients));
        }

        [TestMethod]
        public void CorrectPrintInfo()
        {
            string expStrTopping = "\nKind of ice cream: Sundae\nFlavour: Strawberry\nWeight: 130"
                + $"\nTopping: {topping}"
                + "\nMain ingredients of Sundae:\n"
                + "Milk: 300 grams\n" + "Strawberry: 200 grams\n" + "Cream: 150 grams\n" + "Sugar: 70 grams\n" + $"{topping}: 20 grams\n";
            string expStr = "\nKind of ice cream: Sundae\nFlavour: Strawberry\nWeight: 120"
                + $"\nTopping: Without topping"
                + "\nMain ingredients of Sundae:\n"
                + "Milk: 300 grams\n" + "Strawberry: 200 grams\n" + "Cream: 150 grams\n" + "Sugar: 70 grams\n";
            Assert.AreEqual(expStrTopping, sundaeWithTopping.GetFullInfo());
            Assert.AreEqual(expStr, sundaeWithoutTopping.GetFullInfo());
        }
    }
}
