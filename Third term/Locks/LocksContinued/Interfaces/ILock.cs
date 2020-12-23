namespace LocksContinued
{
    public interface ILock
    {
        void Lock();

        void Unlock();
    }

    public interface IMaybeLock
    {
        bool TryLock(long patienceInMs);

        void Unlock();
    }

    public class SimpleLock:ILock
    {
        public void Lock()
        {
            
        }

        public void Unlock()
        {
            
        }
    }
}