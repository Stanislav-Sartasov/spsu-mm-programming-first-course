using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocksContinued.Monitors
{
    class LockedQueue<T>
    {
        object sync = new object();
        T[] items;
        int tail, head, count;
        public LockedQueue(int capacity)
        {
            items = new T[capacity];
        }

        public void Enqueue(T x)
        {
            Monitor.Enter(sync);
            try
            {
                while (count == items.Length)
                    Monitor.Wait(sync);
                items[tail] = x;
                if (++tail == items.Length)
                    tail = 0;
                ++count;
                Monitor.PulseAll(sync);
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }

        public T Dequeue()
        {
            Monitor.Enter(sync);
            try
            {
                while (count == 0)
                    Monitor.Wait(sync);
                T x = items[head];
                if (++head == items.Length)
                    head = 0;
                --count;
                Monitor.Pulse(sync);
                return x;
            }
            finally
            {
                Monitor.Exit(sync);
            }
        }
    }
}
