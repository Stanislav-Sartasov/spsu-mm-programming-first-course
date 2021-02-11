using ExamSystem.Interfaces;
using System.Threading;

namespace ExamSystem.Locks
{
    public class MutexLock: ILock
    {
        Mutex mutex;

        public MutexLock()
        {
            mutex = new Mutex();
        }

        public void Lock() => mutex.WaitOne();
        
        public void Unlock() => mutex.ReleaseMutex();
    }
}
