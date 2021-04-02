using Microsoft.VisualStudio.TestTools.UnitTesting;
using IceCreamRecipes;

namespace SimpleHierarchy.Tests
{
	[TestClass]
	public class VanillaIceCreamTests
	{
		[TestMethod]
		public void VanillaTestsTop()
		{
			VanillaIceCream vanillaWithTop = new VanillaIceCream("chocolate topping");
			Assert.AreEqual(vanillaWithTop.Name, "Vanilla");
			Assert.AreEqual(vanillaWithTop.Cone, false);
			(string, uint)[] testMas = {("cream", 60), ("milk", 25), ("vanilla sugar", 5), ("sugar", 10)};
			CollectionAssert.AreEqual(vanillaWithTop.Ingredients, testMas);
			string getInfoTest = "Name: Vanilla ice cream with chocolate topping\n" +
				"Recipe:\n" + "cream 60 gram\n" + "milk 25 gram\n" + 
				"vanilla sugar 5 gram\n" + "sugar 10 gram\n" + 
				"chocolate topping 5 gram\n";
			Assert.AreEqual(vanillaWithTop.GetInfo() + vanillaWithTop.GetRecipe(), getInfoTest);
		}
		[TestMethod]
		public void VanillaTests()
		{
			VanillaIceCream vanillaIceCream = new VanillaIceCream();
			Assert.AreEqual(vanillaIceCream.Name, "Vanilla");
			Assert.AreEqual(vanillaIceCream.Cone, false);
			(string, uint)[] testMas = { ("cream", 60), ("milk", 25), ("vanilla sugar", 5), ("sugar", 10) };
			CollectionAssert.AreEqual(vanillaIceCream.Ingredients, testMas);
			string getInfoTest = "Name: Vanilla ice cream\n" +
				"Recipe:\n" + "cream 60 gram\n" + "milk 25 gram\n" +
				"vanilla sugar 5 gram\n" + "sugar 10 gram\n";
			Assert.AreEqual(vanillaIceCream.GetInfo() + vanillaIceCream.GetRecipe(), getInfoTest);
		}
	}
}