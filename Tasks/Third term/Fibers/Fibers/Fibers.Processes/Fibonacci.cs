using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fibers.Processes
{
    class Fibonacci : IProcess
    {
        private int n = 25;
        public async void Process()
        {
            int res = Count(n);
            await Task.Delay(100);
            Console.WriteLine(res);
        }
        private int Count(int x)
        {
            Console.WriteLine($"Fibonacci {x}");
            if (x == 1 || x == 2)
                return 1;
            return Count(x - 1) + Count(x - 2);
        }
    }
}
