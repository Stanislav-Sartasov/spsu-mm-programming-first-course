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

            string expected = "Name - Ice cream in a waffle cup\n" + "Type - Creamy\n" + "Form - In a waffle cup\n" +
                              "Weight gained - 500\n" + "The presence of balls - without balls\n" +
                              "Recipe:\n" + "Necessary products - with 4 eggs, with 70 grams of sugar, with 200 milliliters of milk, " +
                              "with 200 grams of whipped cream, with 7 grams of vanilin, without dark chocolate, without milk chocolate." + "\n*****\n";
            string actual = cream.ShowRecipeAndInfo();

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
            int diff = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                if (!actual[i].Equals(expected[i]))
                    diff++;
            }
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void IceCreamCakeTest()
        {
            IceCreamCake cream = new IceCreamCake();

            string expected = "Name - Ice cream cake\n" + "Type - Cake\n" + "Form - Cake\n" +
                              "Weight gained - 750\n" + "The presence of balls - without balls\n" +
                              "Recipe:\n" + "Necessary products - with 4 eggs, with 100 grams of sugar, without milk, " +
                              "with 500 grams of whipped cream, with 100 grams of dark chocolate, without milk chocolate." + "\n*****\n";
            string actual = cream.ShowRecipeAndInfo();

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
            Assert.AreEqual(expected.Length, actual.Length);
            int diff = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                if (!actual[i].Equals(expected[i]))
                    diff++;
            }
            Assert.AreEqual(0, diff);
        }
        [TestMethod]
        public void PopsicleTest()
        {
            Popsicle cream = new Popsicle();

            string expected = "Name - Popsicle\n" + "Type - Ice cream in a chocolate glaze\n" + "Form - On a wooden stick\n" +
                              "Weight gained - 800\n" + "The presence of balls - without balls\n" + 
                              "Recipe:\n" + "Necessary products - with 4 eggs, with 100 grams of sugar, with 250 milliliters of milk, " +
                              "with 250 grams of whipped cream, with 100 grams of butter, with 150 grams of dark chocolate, without milk chocolate." + "\n*****\n";
            string actual = cream.ShowRecipeAndInfo();

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
            Assert.AreEqual(expected.Length, actual.Length);
            int diff = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                if (!actual[i].Equals(expected[i]))
                    diff++;
            }
            Assert.AreEqual(0, diff);
        }
    }
}
