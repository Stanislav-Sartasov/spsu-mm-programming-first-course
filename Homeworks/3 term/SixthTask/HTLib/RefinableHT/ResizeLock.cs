using System.Threading;

namespace HTLib
{
	public class ResizeLock
	{
		public volatile Thread owner;
		public volatile Mutex mutex;

		public ResizeLock()
		{
			owner = null;
			mutex = new Mutex();
		}

		public void Lock()
		{
			mutex.WaitOne();
		}
		public void Unlock()
		{
			mutex.ReleaseMutex();
		}

		public bool IsEmpty
		{
			get 
			{ 
				return owner == null; 
			}
		}
		public bool ResizeNotAvaiable(Thread currThread)
		{
			return (!IsEmpty) && (owner != currThread);
		}

		public void SetOwner(Thread currThread)
		{
			owner = currThread;
		}

		public void Reset()
		{
			Lock();
			owner = null;
			Unlock();
		}
	}
}