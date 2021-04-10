using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Task4
{
	public class ThreadPool : IDisposable
	{
		const int ThreadCount = 16;

		bool stop = false;
		bool disposed = false;
		Queue<Action> taskPool = new Queue<Action>();
		List<Thread> threads = new List<Thread>();
		List<string> log = new List<string>();

		public List<string> Log
		{
			get
			{
				List<string> local = new List<string>();
				lock (log)
					foreach (var s in log)
						local.Add(s);
				return local;
			}
		}

		void AddLog(string str)
		{
			lock (log)
				log.Add(str);
		}

		public ThreadPool()
		{
			lock (threads)
			{
				for (int i = 0; i < ThreadCount; i++)
				{
					Thread thread = new Thread(Work)
					{
						Name = i.ToString(),
						IsBackground = true,
					};
					threads.Add(thread);
					thread.Start();
				}
			}
		}

		void Work()
		{
			while (!stop)
			{
				Monitor.Enter(taskPool);
				if (taskPool.Count > 0)
				{
					Action task = taskPool.Dequeue();
					AddLog($"Thread[{Thread.CurrentThread.Name}]\ttook task:\t{task.Method.Name}\tat {{{DateTime.Now}}}");
					Monitor.PulseAll(taskPool);
					Monitor.Exit(taskPool);
					task?.Invoke();
				}
				else
				{
					Monitor.Wait(taskPool);
					Monitor.Exit(taskPool);
				}
			}
		}

		public void Enqueue(Action action)
		{
			lock (taskPool)
			{
				if (stop)
					return;
				taskPool.Enqueue(action);
				Monitor.PulseAll(taskPool);
			}
		}

		public void Dispose()
		{
			if (!disposed)
			{
				lock (taskPool)
				{
					stop = true;
					Monitor.PulseAll(taskPool);
				}

				lock (threads)
					foreach (var thread in threads)
						thread.Join();

				taskPool.Clear();
				threads.Clear();

				disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		~ThreadPool()
		{
			Dispose();
		}
	}
}
