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
				if (queue.Count == 0)
				{
					queue.Enqueue(a);
					for (int i = 0; i < number; i++)
					{
						if (!threadLst[i].IsAlive)
						{
							threadLst[i].Join();
							threadLst[i] = new Thread(Work);
							threadLst[i].Name = i.ToString();
							threadLst[i].Start();
						}
					}
				}
				else
					queue.Enqueue(a);
			}
		}
		private void Work()
		{
				for (int i = 0; i < queue.Count; ++i)
				{
					Action act = null;
					bool flag;
					flag = false;
					lock (queue)
					{
						if (queue.Count > 0)
						{
						act = queue.Dequeue();
						flag = true;
						}
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
