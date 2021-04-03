using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Task3
{
	class Consumer : IActor
	{
		public Consumer(Depot<Product> depot, int sleepTime, string id)
		{
			this.depot = depot ?? throw new Exception("depot can\'t be null");
			if (sleepTime < 0)
				throw new Exception("sleep time can\'t be less than 0");
			this.sleepTime = sleepTime;
			Id = id ?? throw new Exception("dealer id can\'t be null");
		}

		Depot<Product> depot;
		List<Product> storage = new List<Product>();
		public Task Session { get; private set; } = null;
		public string Id { get; private set; }
		int sleepTime;
		bool stop = false;

		public List<Product> LocalStorage
		{
			get
			{
				List<Product> local = new List<Product>();
				foreach (var p in storage)
					local.Add(p);
				return local;
			}
		}
		public void Start()
		{
			stop = false;
			Session = Task.Run(Work);
		}
		void Work()
		{
			while (!stop)
			{
				Product product = depot.Get() ?? Product.Default;
				storage.Add(product);
				Thread.Sleep(sleepTime);
			}
		}
		public void Stop()
		{
			stop = true;
		}
	}
}
