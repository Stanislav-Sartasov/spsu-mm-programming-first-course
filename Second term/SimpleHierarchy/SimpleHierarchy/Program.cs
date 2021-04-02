using IceCreamRecipes;

namespace SimpleHierarchy
{
	class Program
	{
		static void Main(string[] args)
		{
			FruitIceCream fruitIce = new FruitIceCream();
			fruitIce.GetFullInfo();
			VanillaIceCream vanillaIceTop = new VanillaIceCream("chocolate topping");
			vanillaIceTop.GetFullInfo();
			VanillaIceCream vanillaIce = new VanillaIceCream();
			vanillaIce.GetFullInfo();
		}
	}
}
