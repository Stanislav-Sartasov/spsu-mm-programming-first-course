using System.Collections.Generic;
using AbstractIceCream;

namespace IceCreamRecipes
{
    public class FruitIce : IceCream
    {
        public FruitIce() : base("FruitIce", "Kiwi", 60, new Dictionary<string, int> { { "Water", 200 }, { "Kiwi", 150 }, { "Sugar", 20 }, { "Lemon juice", 50 } })
        { }
    }
}
