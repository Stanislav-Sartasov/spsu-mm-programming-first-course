using ExamSystem.Interfaces;
using System.Threading;

namespace ExamSystem.Locks
{
    public class TASLock : ILock
    {
        volatile int state = 0;

        public void Lock()
        {
            while (Interlocked.CompareExchange(ref state, 1, 0) == 1) { }
        }

        public void Unlock() => state = 0;
    }
}
