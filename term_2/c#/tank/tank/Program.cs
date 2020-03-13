using System;
using manyTanks;
using abstractTank;

namespace tank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("First tank:");
            anyTank tank = new t34();
            tank.getInfo();

            Console.WriteLine("Second tank:");
            
            anyTank secTank = new m4();
            secTank.getInfo();

        }
    }
}
