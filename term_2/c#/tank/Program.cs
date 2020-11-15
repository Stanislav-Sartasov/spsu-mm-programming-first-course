using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractTanks;
using TankT34;
using TankM4;

namespace tank
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("First tank:");
			AbstractTank tank = new T34();
			Console.WriteLine(tank.getInfo());

			Console.WriteLine("Second tank:");

			AbstractTank secTank = new M4();
			Console.WriteLine(secTank.getInfo());
			Console.ReadKey();
		}
	}
}
	