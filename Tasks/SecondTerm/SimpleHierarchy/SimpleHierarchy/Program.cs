using System;
using SimpleHierarchy.ClassesHeirs;

namespace SimpleHierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            BananaIceCream bananaIceCream = new BananaIceCream();
            Console.WriteLine(bananaIceCream.ShowRecipe());

            ChocolateIceCream chocolateIceCream = new ChocolateIceCream();
            Console.WriteLine(chocolateIceCream.ShowRecipe());
        }
    }
}
