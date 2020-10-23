using LocksContinued.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocksContinued.Barriers
{
    class SenseBarrier:IBarrier
    {
        volatile int count;
        int size;
        volatile bool sense = false;
        ThreadLocal<bool> threadSense;

        public SenseBarrier(int n)
        {
            count = n;
            size = n;
            sense = false;
            threadSense = new ThreadLocal<bool>(() => !sense);
        }

        public void Await()
        {
            bool mySense = threadSense.Value;
            int position = Interlocked.Decrement(ref count);
            if (position == 0)
            {
                count = size;
                sense = mySense;
            }
            else
            {
                while (sense != mySense) { }
            }
            threadSense.Value = !mySense;
        }
    }
}
