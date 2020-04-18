using System;
using System.Collections.Generic;
using AbstractIceCream;

namespace IceCreamRecipes
{
    public class Sundae : IceCream
    {
        public string Topping { get; set; }

        public Sundae() : base("Sundae", "Strawberry", 120, new Dictionary<string, int> { { "Milk", 300 }, { "Strawberry", 200 }, { "Cream", 150 }, { "Sugar", 70 } })
        {
            Topping = "Without topping";
        }

        public Sundae(string topping) : base("Sundae", "Strawberry", 130, new Dictionary<string, int> { { "Milk", 300 },{ "Strawberry", 200}, { "Cream", 150 }, { "Sugar", 70 }, { topping,  20} })
        {
            Topping = topping;
        }

        public override string GetMainInfo()
        {
            return base.GetMainInfo() + $"\nTopping: {Topping}";
        }
    }
}
