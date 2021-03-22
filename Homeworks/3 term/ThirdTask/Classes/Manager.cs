using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThirdTask
{
	public class Manager
	{
		private Producer[] producers;
		private Consumer[] consumers;
		private Mutex mutex;

		public List<int> Data { get; private set; }

		public Manager(int producersNum, int consumersNum)
		{
			mutex = new Mutex();
			Data = new List<int>();

			producers = new Producer[producersNum];
			consumers = new Consumer[consumersNum];
		}

		public void Start(int consFlag)
		{
			if (consFlag == 0)
			{
				for (int i = 0; i < producers.Length; i++)
				{
					producers[i] = new Producer($"Producer [{i + 1}]", this);
				}
			}
			else
			{
				for (int i = 0; i < consumers.Length; i++)
				{
					consumers[i] = new Consumer($"Consumer [{i + 1}]", this);
				}
			}
		}

		public void Add(string name)
		{
			mutex.WaitOne();
			try
			{
				Data.Add(5);
				Console.WriteLine($"{name} has added an element.");
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}

		public void Remove(string name)
		{
			mutex.WaitOne();
			try
			{
				if (Data.Count != 0)
				{
					Data.RemoveAt(0);
					Console.WriteLine($"{name} has removed an element.");
				}
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}

		public void Finish(int consFlag)
		{
			if (consFlag == 0)
			{
				for (int i = 0; i < producers.Length; i++)
				{
					producers[i].Join();
				}
			}
			else
			{
				for (int i = 0; i < consumers.Length; i++)
				{
					consumers[i].Join();
				}
			}
		}
	}
}