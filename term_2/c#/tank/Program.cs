using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("First tank:");
            abstractTank tank = new t34();
            Console.WriteLine(tank.getInfo());

            Console.WriteLine("Second tank:");

            abstractTank secTank = new m4();
            Console.WriteLine(secTank.getInfo());
            Console.ReadKey();
        }
    }
}
