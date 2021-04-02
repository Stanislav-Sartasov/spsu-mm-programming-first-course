using System;

namespace AbstractClassLibrary
{
	public abstract class IceCream
	{
		public string Name { get; set; }
		public bool Cone { get; set; }
		public (string, uint)[] Ingredients { get; set; }
		public IceCream(string name, bool cone, params (string, uint)[] ingredients)
		{
			Name = name;
			Cone = cone;
			Ingredients = ingredients;
		}
		public virtual string GetInfo()
		{
			if (Cone)
				return $"Name: {Name} ice cream in a waffle cone";
			return $"Name: {Name} ice cream";
		}
		public virtual string GetRecipe()
		{
			string recipe = "\nRecipe:\n";
			foreach ((string, int) i in Ingredients)
			{
				recipe += i.Item1 + " " + i.Item2 + " gram\n";
			}
			return recipe;
		}
		public void GetFullInfo()
		{
			Console.WriteLine(GetInfo() + GetRecipe());
		}
	}
}