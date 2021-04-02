using Microsoft.VisualStudio.TestTools.UnitTesting;
using IceCreamRecipes;

namespace SimpleHierarchy.Tests
{
	[TestClass]
	public class FruitIceCreamTests
	{
		[TestMethod]
		public void FruitIceTests()
		{
			FruitIceCream fruitIceCream = new FruitIceCream();
			Assert.AreEqual(fruitIceCream.Name, "Fruit");
			Assert.AreEqual(fruitIceCream.Cone, true);
			(string, uint)[] testMas = {("blackberry", 15), ("kiwi", 15), ("yogurt", 45), ("sugar", 10)};
			CollectionAssert.AreEqual(fruitIceCream.Ingredients, testMas);
			string getInfoTest = "Name: Fruit ice cream in a waffle cone\n" +
						  "Recipe:\n" + "blackberry 15 gram\n" + "kiwi 15 gram\n" +
						  "yogurt 45 gram\n" + "sugar 10 gram\n";
			Assert.AreEqual(fruitIceCream.GetInfo() + fruitIceCream.GetRecipe(), getInfoTest);
		}
	}
}
