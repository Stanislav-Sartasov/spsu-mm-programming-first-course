using System;
using SomeIceCreams;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            SpecialInTheHorn iceCream = new SpecialInTheHorn();
            Console.WriteLine(iceCream.GetRecipe());
            Console.CursorVisible = false;
            while (Console.ReadKey().Key != ConsoleKey.Enter) ;
        }
    }
}