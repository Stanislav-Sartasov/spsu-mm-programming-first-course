using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocksContinued.WorkStealing
{
    public class BDEQueue : IDEQueue<Task>
    {
        Task[] tasks;
        volatile int bottom;
        AtomicStampedReference<int> top;
        public BDEQueue(int capacity)
        {
            tasks = new Task[capacity];
            top = new AtomicStampedReference<int>(0, 0);
            bottom = 0;
        }
        public void PushBottom(Task t)
        {
            tasks[bottom] = t;
            Interlocked.Increment(ref bottom);
        }

        // called by thieves to determine whether to try to steal
        public bool IsEmpty()
        {
            int localTop = top.GetReference();
            int localBottom = bottom;
            return localBottom <= localTop;
        }

        // for thief
        public Task PopTop() 
        {
            int stamp = 0;
            int oldTop = top.Get(out stamp), newTop = oldTop + 1;
            int oldStamp = stamp, newStamp = oldStamp + 1;
            if (bottom <= oldTop)
                return null;
            Task t = tasks[oldTop];
            if (top.CompareAndSet(oldTop, newTop, oldStamp, newStamp))
                return t;
            return null;
        }

        // for ordinary thread
        public Task PopBottom() 
        {
            if (bottom == 0)
                return null;
            Interlocked.Decrement(ref bottom);
            Task t = tasks[bottom];
            int stamp;
            int oldTop = top.Get(out stamp);
            int newTop = 0;
            int oldStamp = stamp, newStamp = oldStamp + 1;
            if (bottom > oldTop)
                return t;
            if (bottom == oldTop)
            {
                bottom = 0;
                if (top.CompareAndSet(oldTop, newTop, oldStamp, newStamp))
                    return t;
            }
            top.Set(newTop, newStamp);
            return null;
        }
    }
}
