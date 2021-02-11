using ExamSystem.Interfaces;

namespace ExamSystem.Locks
{
    public class TASLockFactory : ILockFactory
    {
        public ILock CreateLock() => new TASLock();
    }
}
