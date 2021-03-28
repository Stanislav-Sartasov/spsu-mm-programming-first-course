using System;
using System.Collections.Generic;
using System.Threading;

namespace TPLib
{
	public class ThreadPool : IDisposable
	{
		private const int numOfThreads = 4;

		private Queue<Action> tasks;
		public List<Thread> ListOfThreads { get; private set; }

		private volatile bool isFinished;
		private bool isDisposed;
		private readonly object obj;

		public void Enqueue(Action action)
		{
			if (!isFinished)
			{
				Monitor.Enter(obj);
				try
				{
					tasks.Enqueue(action);
					Monitor.PulseAll(obj);
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}
			else
			{
				Console.WriteLine("Error! Thread pool has been disposed.");
			}
		}

		public void StartTask()
		{
			while(!isFinished)
			{
				Monitor.Enter(obj);
				try
				{
					if (isFinished)
					{
						return;
					}
					else if (tasks.Count == 0)
					{
						Monitor.Wait(obj);
					}
					else
					{
						var action = tasks.Dequeue();
						Console.WriteLine($"{Thread.CurrentThread.Name} is working on task...");

						action?.Invoke();
					}
				}
				finally
				{
					Monitor.Exit(obj);
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool isDisposing)
		{
			if (isDisposed)
			{
				return;
			}

			isFinished = true;
			Monitor.Enter(obj);
			try
			{
				Monitor.PulseAll(obj);
			}
			finally
			{
				Monitor.Exit(obj);
			}

			foreach(var thread in ListOfThreads)
			{
				thread.Join();
			}

			if(isDisposing)
			{
				tasks.Clear();
				ListOfThreads.Clear();
			}
			isDisposed = true;
		}

		~ThreadPool() => Dispose(false);

		public ThreadPool()
		{
			tasks = new Queue<Action>();
			ListOfThreads = new List<Thread>();

			isFinished = false;
			isDisposed = false;

			obj = new object();

			for (int i = 0; i < numOfThreads; i++)
			{
				ListOfThreads.Add(new Thread(StartTask) { Name = $"Thread [{i}]", IsBackground = true });
				ListOfThreads[i].Start();
			}
		}
	}
}