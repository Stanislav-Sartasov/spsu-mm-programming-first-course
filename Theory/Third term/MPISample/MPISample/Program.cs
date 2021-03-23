using MPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPISample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var env = new MPI.Environment(ref args))
            {
                Intracommunicator world = Communicator.world;
                int rank = world.Rank;

                int result = world.Reduce(rank, Operation<int>.Add, 0);
                if(world.Rank == 0)
                {
                    Console.WriteLine(result);
                }
                
            }
        }
    }
}
