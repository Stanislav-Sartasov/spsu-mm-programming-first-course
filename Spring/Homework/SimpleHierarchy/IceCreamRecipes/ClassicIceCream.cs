using System.Collections.Generic;
using AbstractIceCream;

namespace IceCreamRecipes
{
    public class ClassicIceCream : IceCream
    {
        public ClassicIceCream() : base("Milk ice cream", "Vanilla", 80, new Dictionary<string, int> { { "Cream", 300 }, { "Milk", 200 }, { "Egg yolk", 40 }, { "Powdered sugar", 25 }, {"Vanilla", 25 } })
        { }
    }
}
