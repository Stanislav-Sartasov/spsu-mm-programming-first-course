using System;
using SimpleHierarchy.Origin;

namespace SimpleHierarchy.ClassesHeirs
{
    public class BananaIceCream : AbstractOriginIceCream
    {
        public int Bananas { get; protected set; }

        public BananaIceCream()
        {
            IceCreamName = "Banana Ice Cream";
            Weight = 150;
            Milk = 60;
            Eggs = 2;
            Sugar = 10;
            Cream = 30;
            Bananas = 1;
        }

        public override string ShowRecipe()
        {
            string recipe = base.ShowRecipe() + $" Number of bananas - {Bananas}\n";
            return recipe;
        }
    }
}