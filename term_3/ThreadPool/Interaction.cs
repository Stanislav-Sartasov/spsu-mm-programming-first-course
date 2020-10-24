using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    class Interaction
    {
        public int GetInt()
        {
            int target;
            if (Int32.TryParse(Console.ReadLine(), out target))
            {
                return target;
            }
            else
            {
                Console.WriteLine("int is required");

            }
            return 0;
        }
    }
}
