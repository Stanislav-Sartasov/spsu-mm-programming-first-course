using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using MPI;
using System.Threading.Tasks;

namespace Task2
{
	class Program
	{
		public static void Main(string[] args)
		{
			using (MPI.Environment env = new MPI.Environment(ref args))
			{
				Intracommunicator world = Communicator.world;

				if (world.Rank == 0)
					SuperVisor.Start(world, args);
				else
					Core.Start(world);

				Console.WriteLine($"core[{world.Rank}] finished");
			}
		}
	}
}
