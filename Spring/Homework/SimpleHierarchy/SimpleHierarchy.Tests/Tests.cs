using Microsoft.VisualStudio.TestTools.UnitTesting;
using IceCreamRecipes;
using System.Collections.Generic;
using System.Linq;

namespace SimpleHierarchy.Tests
{
    [TestClass]
    public class FruitIceTest
    {
        FruitIce fruitIce = new FruitIce();
        Dictionary<string, int> expIngredients =new Dictionary<string, int> { { "Water", 200 }, { "Kiwi", 150 }, { "Sugar", 20 }, { "Lemon juice", 50 } };
        
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
        public void CorrectServingWidth()
        {
            Assert.AreEqual(60, fruitIce.ServingWeight);
        }

        [TestMethod]
        public void CorrectIngredients()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(expIngredients, fruitIce.Ingredients));
        }

    }

    [TestClass]
    public class SundaeTest
    {
        public Sundae sundaeWithTopping = new Sundae("Chocolate");
        public Sundae sundaeWithoutTopping = new Sundae();
        Dictionary<string, int> expIngredients = new Dictionary<string, int> { { "Milk", 300 }, { "Strawberry", 200 }, { "Cream", 150 }, { "Sugar", 70 } };
        Dictionary<string, int> expIngredientsTopping = new Dictionary<string, int> { { "Milk", 300 }, { "Strawberry", 200 }, { "Cream", 150 }, { "Sugar", 70 }, { "Chocolate", 20 } };

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
        public void CorrectServingWidth()
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
    }

    [TestClass]
    public class ClassicIceCreamTest
    {
        ClassicIceCream classicIC = new ClassicIceCream();
        Dictionary<string, int> expIngredients = new Dictionary<string, int> { { "Cream", 300 }, { "Milk", 200 }, { "Egg yolk", 40 }, { "Powdered sugar", 25 }, { "Vanilla", 25 } };

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
        public void CorrectServingWidth()
        {
            Assert.AreEqual(80, classicIC.ServingWeight);
        }

        [TestMethod]
        public void CorrectIngredients()
        {
            Assert.IsTrue(Enumerable.SequenceEqual(expIngredients, classicIC.Ingredients));
        }
    }
}
