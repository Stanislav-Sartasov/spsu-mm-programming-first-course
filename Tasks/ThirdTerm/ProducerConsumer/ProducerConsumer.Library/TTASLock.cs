using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer.Library
{
    public static class TTASLock
    {
        static volatile int state = 0;

        public static void Lock()
        {
            while (Interlocked.CompareExchange(ref state, 1, 0) == 1)
            {
                Thread.Sleep(0);
            }
        }

        public static void Unlock()
        {
            state = 0;
        }
    }
}
