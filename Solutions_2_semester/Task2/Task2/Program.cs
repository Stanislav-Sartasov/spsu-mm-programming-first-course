using System;
using PopularIceCream;
using IceCream;

namespace Task2
{
    class TestIceCreamCreamyBriquetteCount70 : AbstractIceCream
    {
        public TestIceCreamCreamyBriquetteCount70()
        {
            type = Type.special;
            innings = Innings.inTheHorn;
            count = 0xf0;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            TestIceCreamCreamyBriquetteCount70 iceCream = new TestIceCreamCreamyBriquetteCount70();
            Console.WriteLine(iceCream.GetRecipe());
            while (Console.ReadKey().Key != ConsoleKey.Enter) ;
        }
    }
}