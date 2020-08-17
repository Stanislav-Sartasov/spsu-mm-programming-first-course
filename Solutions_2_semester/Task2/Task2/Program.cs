using System;
using SomeIceCreams;

namespace Task2
{
    class Program
    {

        static void Main(string[] args)
        {
            AbstractIceCream.AbstractIceCream iceCream = new Special();
            Console.WriteLine(iceCream.GetRecipe());
            Console.CursorVisible = false;
            while (Console.ReadKey().Key != ConsoleKey.Enter) ;
        }
    }
}