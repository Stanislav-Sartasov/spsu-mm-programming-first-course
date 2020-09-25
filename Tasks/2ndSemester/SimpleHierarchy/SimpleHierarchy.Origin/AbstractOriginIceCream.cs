using System;

namespace SimpleHierarchy.Origin
{
    public abstract class AbstractOriginIceCream
    {
        public string IceCreamName { get; protected set; }
        public int Weight { get; protected set; }
        public int Milk { get; protected set; }
        public int Eggs { get; protected set; }
        public int Sugar { get; protected set; }
        public int Cream { get; protected set; }

        public virtual string ShowRecipe()
        {
            string recipe = $" Name - {IceCreamName}\n" +
                            $" Weighs - {Weight}\n" +
                            $" Gram of milk - {Milk}\n" +
                            $" Number of eggs - {Eggs}\n" +
                            $" Gram of sugar - {Sugar}\n" +
                            $" Gram of Cream - {Cream}\n";
            return recipe;
        }
    }
}
