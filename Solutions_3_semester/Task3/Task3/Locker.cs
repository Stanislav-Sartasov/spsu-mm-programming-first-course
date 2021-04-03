using System.Threading;

namespace Task3
{
	class Locker
	{
		int state = 0;
		public void Lock()
		{
			while (true)
			{
				while (state == 1)
					Thread.Sleep(0);
				if (Interlocked.CompareExchange(ref state, 1, 0) == 0)
					return;
			}
		}
		public void Unlock()
		{
			state = 0;
		}
	}
}
