using SimpleHierarchyInheritorClasses;
using System;

namespace SimpleHierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            Cream iceCream = new Cream();
            iceCream.ShowRecipeAndInfo();

            Popsicle popsicle = new Popsicle();
            popsicle.ShowRecipeAndInfo();

            IceCreamCake iceCreamCake = new IceCreamCake();
            iceCream.ShowRecipeAndInfo();

            Console.ReadKey();
        }
    }
}
