using System;
using VarietiesOfTanks;

namespace Task2Tank
{
    class Program
    {
        static void Main(string[] args)
        {
            PantherTank tankSdKfz = new PantherTank("Sd.Kfz. 267", "Medium Tank", "Germany", 44.8F, 6.87F, 2.99F, 5, 720);
            Console.WriteLine("Info about Sd.Kfz. 267:");
            Console.WriteLine(tankSdKfz.GetInfo());

            TigerTank tankPzKpfw = new TigerTank("Pz.Kpfw VI", "Heavy tank", "Germany", 54, 6.316F, 3, 5, 45.4F);
            Console.WriteLine("\nInfo about Pz.Kpfw VI:");
            Console.WriteLine(tankPzKpfw.GetInfo());
            Console.ReadKey();
        }
    }
}
