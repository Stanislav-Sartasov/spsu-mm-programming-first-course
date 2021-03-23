using LocksContinued.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocksContinued.Barriers
{
    public class SimpleBarrier : IBarrier
    {
        volatile int count;
        int size;
        public SimpleBarrier(int n)
        {
            count = n;
            size = n;
        }

        public void Await()
        {
            int position = Interlocked.Decrement(ref count);
            if (position == 0)
            {
                count = size;
            }
            else
            {
                while (count != 0) { };
            }
        }
    }
}
