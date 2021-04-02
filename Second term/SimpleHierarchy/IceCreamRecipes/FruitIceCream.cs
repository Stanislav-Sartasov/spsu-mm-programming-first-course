using AbstractClassLibrary;

namespace IceCreamRecipes
{
	public class FruitIceCream : IceCream
	{
		public FruitIceCream() : base("Fruit", true, ("blackberry", 15), ("kiwi", 15), ("yogurt", 45), ("sugar", 10))
		{
		}
	}
}
