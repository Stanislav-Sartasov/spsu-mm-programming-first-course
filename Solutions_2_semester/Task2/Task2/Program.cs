using System;
using PopularIceCream;
using IceCream;

namespace Task2
{
    class TestIceCreamSpeciallinTheHornCount0xf0 : AbstractIceCream
    {
        public TestIceCreamSpeciallinTheHornCount0xf0()
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
            TestIceCreamSpeciallinTheHornCount0xf0 iceCream = new TestIceCreamSpeciallinTheHornCount0xf0();
            Console.WriteLine(iceCream.GetRecipe());
            while (Console.ReadKey().Key != ConsoleKey.Enter) ;
        }
    }
}