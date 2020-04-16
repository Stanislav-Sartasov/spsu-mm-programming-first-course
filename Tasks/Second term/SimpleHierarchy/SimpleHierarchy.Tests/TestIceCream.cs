using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleHierarchy.InheritorClasses;

namespace SimpleHierarchy.Tests
{
    [TestClass]
    public class TestIceCream
    {
        [TestMethod]
        public void CreamTest()
        {
            Cream cream = new Cream();
            Assert.AreEqual("Ice cream in a waffle cup", cream.IceCreamName);
            Assert.AreEqual("Creamy", cream.Type);
            Assert.AreEqual("In a waffle cup", cream.Form);
            Assert.AreEqual((uint)0, cream.NumberOfBalls);
            Assert.AreEqual((uint)500, cream.Weight);
            Assert.AreEqual((uint)0, cream.WeightOfDarkChocolate);
            Assert.AreEqual((uint)0, cream.WeightOfMilkChocolate);
            Assert.AreEqual((uint)200, cream.Milk);
            Assert.AreEqual((uint)4, cream.Eggs);
            Assert.AreEqual((uint)70, cream.Sugar);
            Assert.AreEqual((uint)200, cream.WhippedCream);
            Assert.AreEqual((uint)7, cream.Vanilin);
        }
        [TestMethod]
        public void IceCreamCakeTest()
        {
            IceCreamCake cream = new IceCreamCake();
            Assert.AreEqual("Ice cream cake", cream.IceCreamName);
            Assert.AreEqual("Cake", cream.Type);
            Assert.AreEqual("Cake", cream.Form);
            Assert.AreEqual((uint)0, cream.NumberOfBalls);
            Assert.AreEqual((uint)750, cream.Weight);
            Assert.AreEqual((uint)100, cream.WeightOfDarkChocolate);
            Assert.AreEqual((uint)0, cream.WeightOfMilkChocolate);
            Assert.AreEqual((uint)0, cream.Milk);
            Assert.AreEqual((uint)4, cream.Eggs);
            Assert.AreEqual((uint)100, cream.Sugar);
            Assert.AreEqual((uint)500, cream.WhippedCream);
        }
        [TestMethod]
        public void PopsicleTest()
        {
            Popsicle cream = new Popsicle();
            Assert.AreEqual("Popsicle", cream.IceCreamName);
            Assert.AreEqual("Ice cream in a chocolate glaze", cream.Type);
            Assert.AreEqual("On a wooden stick", cream.Form);
            Assert.AreEqual((uint)0, cream.NumberOfBalls);
            Assert.AreEqual((uint)800, cream.Weight);
            Assert.AreEqual((uint)150, cream.WeightOfDarkChocolate);
            Assert.AreEqual((uint)0, cream.WeightOfMilkChocolate);
            Assert.AreEqual((uint)250, cream.Milk);
            Assert.AreEqual((uint)4, cream.Eggs);
            Assert.AreEqual((uint)100, cream.Sugar);
            Assert.AreEqual((uint)250, cream.WhippedCream);
            Assert.AreEqual((uint)100, cream.Butter);
        }
    }
}
