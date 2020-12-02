using System;
using SimpleHierarchy.Origin;

namespace SimpleHierarchy.ClassesHeirs
{
    public class ChocolateIceCream : AbstractOriginIceCream
    {
        public int Chocolate { get; protected set; }

        public ChocolateIceCream()
        {
            IceCreamName = "Chocolate Ice Cream";
            Weight = 900;
            Milk = 300;
            Eggs = 3;
            Sugar = 85;
            Cream = 300;
            Chocolate = 100;
        }

        public override string ShowRecipe()
        {
            string recipe = base.ShowRecipe() + $" Gram of Chocolate - {Chocolate}\n";
            return recipe;
        }
    }
}
