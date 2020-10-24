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
			lock (queue)
			{
				queue.Enqueue(a);
			}
		}
		private void Work()
		{
			while (!stop)
			{
				for (int i = 0; i < queue.Count; ++i)
				{
					Action act;
					lock (queue)
					{
						act = queue.Dequeue();
					}
					act?.Invoke();
				}	
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
				lock(queue)
				{
					queue.Clear();
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
