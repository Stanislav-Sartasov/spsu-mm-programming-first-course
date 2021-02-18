using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
	public class MyThreadPool : IDisposable
	{
		private int number;
		private Queue<Action> queue;
		private List<Thread> threadLst;
		private volatile bool stop;
		public MyThreadPool(int numberOfThreads)
		{
			queue = new Queue<Action>();
			queue.Clear();
			number = numberOfThreads;
			threadLst = new List<Thread>();
			stop = false;
			for (int i = 0; i < number; i++)
			{
				threadLst.Add(new Thread(Work));
				threadLst[i].Name = i.ToString();
				threadLst[i].Start();
			}
		}
		public void Enqueue(Action a)
		{
			Monitor.Enter(queue);
			try
			{
				queue.Enqueue(a);
				Monitor.PulseAll(queue);
			}
			finally
			{
				Monitor.Exit(queue);
			}
		}
		private void Work()
		{
				while (!stop)
				{
					Action act = null;
					bool flag;
					flag = false;
					Monitor.Enter(queue);
					try
					{
						while ((queue.Count == 0) && (!stop))
							Monitor.Wait(queue);
						if (queue.Count > 0)
						{
							act = queue.Dequeue();
							flag = true;
						}
					}
					finally
					{
						Monitor.Exit(queue);
					}
					if (flag)
						act?.Invoke();
				}	
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool flag)
		{
			stop = true;
			if (flag)
			{
				Monitor.Enter(queue);
				try
				{
					queue.Clear();
					Monitor.PulseAll(queue);
				}
				finally
				{
					Monitor.Exit(queue);
				}
				foreach (var t in threadLst)
				{
					t.Join();
				}
				threadLst.Clear();
			}
		}
		~MyThreadPool()
		{
			Dispose(false);
		}
	}
}
