using SimpleHierarchy.InheritorClasses;
using System;

namespace SimpleHierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            Cream iceCream = new Cream();
            Console.WriteLine(iceCream.ShowRecipeAndInfo());

            Popsicle popsicle = new Popsicle();
            Console.WriteLine(popsicle.ShowRecipeAndInfo());

            IceCreamCake iceCreamCake = new IceCreamCake();
            Console.WriteLine(iceCreamCake.ShowRecipeAndInfo());

            Console.ReadKey();
        }
    }
}
