using AbstractClassLibrary;

namespace IceCreamRecipes
{
	public class VanillaIceCream : IceCream
	{
		public string Topping { get; set; }
		public VanillaIceCream() : base("Vanilla", false, ("cream", 60), ("milk", 25), ("vanilla sugar", 5), ("sugar", 10))
		{
			Topping = "without topping";
		}
		public VanillaIceCream(string topping) : base("Vanilla", false, ("cream", 60), ("milk", 25), ("vanilla sugar", 5), ("sugar", 10))
		{
			Topping = topping;
		}
		public override string GetInfo()
		{
			if (Topping != "without topping")
				return base.GetInfo() + $" with {Topping}";
			return base.GetInfo();
		}
		public override string GetRecipe()
		{
			if (Topping != "without topping")
				return base.GetRecipe() + $"{Topping} 5 gram\n";
			return base.GetRecipe();
		}
	}
}
