using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
	public class TestSystem
	{
		public TestSystem(int dealerCount, int consumerCount, int sleepTime)
		{
			if (dealerCount < 0)
				throw new Exception("dealer count can\'t be less than 0");
			this.dealerCount = dealerCount;
			if (consumerCount < 0)
				throw new Exception("consumer count can\'t be less than 0");
			this.consumerCount = consumerCount;
			if (sleepTime < 0)
				throw new Exception("sleep time can\'t be less than 0");
			this.sleepTime = sleepTime;
		}

		int dealerCount;
		int consumerCount;
		int sleepTime;
		bool stop;
		Task task = null;
		List<Dealer> dealers = null;
		List<Consumer> consumers = null;

		public List<IActor> Dealers
		{
			get
			{
				if (!stop)
					return null;
				return new List<IActor>(dealers.ToArray());
			}
		}
		public List<IActor> Consumers
		{
			get
			{
				if (!stop)
					return null;
				return new List<IActor>(consumers.ToArray());
			}
		}

		public void Start()
		{
			stop = false;

			dealers = new List<Dealer>();
			consumers = new List<Consumer>();
			Depot<Product> depot = new Depot<Product>();

			for (int i = 0; i < dealerCount; i++)
				dealers.Add(new Dealer(depot, sleepTime, "dealer №" + i.ToString()));
			for (int i = 0; i < consumerCount; i++)
				consumers.Add(new Consumer(depot, sleepTime, "consumer №" + i.ToString()));

			foreach (var d in dealers)
				d.Start();
			foreach (var c in consumers)
				c.Start();

			task = Task.Run(() =>
			{
				while (!stop)
					Thread.Sleep(0);

				List<Task> tasks = new List<Task>();

				foreach (var d in dealers)
				{
					d.Stop();
					tasks.Add(d.Session);
				}
				foreach (var c in consumers)
				{
					c.Stop();
					tasks.Add(c.Session);
				}

				Task.WaitAll(tasks.ToArray());
			});
		}

		public void Stop()
		{
			if (task == null)
				return;
			stop = true;
			task.Wait();
		}
	}
}
