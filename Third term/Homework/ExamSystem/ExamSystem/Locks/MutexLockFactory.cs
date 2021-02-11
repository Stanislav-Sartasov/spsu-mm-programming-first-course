using ExamSystem.Interfaces;

namespace ExamSystem.Locks
{
    public class MutexLockFactory : ILockFactory
    {
        public ILock CreateLock() => new MutexLock();
    }
}
