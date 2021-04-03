using System;
using System.Collections.Generic;
using System.Threading;
namespace Task3
{
	class Program
	{
		const int DealerCount = 5;
		const int ConsumerCount = 5;
		const int SleepTime = 500;
		static void Main(string[] args)
		{
			TestSystem test = new TestSystem(DealerCount, ConsumerCount, SleepTime);
			test.Start();
			Console.WriteLine("Started\n");
			while (!Console.KeyAvailable) ;
			Console.WriteLine("Stopping..\n");
			test.Stop();

			foreach (var consumer in test.Consumers)
			{
				Console.WriteLine($"consumer_id = {consumer.Id}: {{");
				foreach (var p in consumer.LocalStorage)
					Console.WriteLine($"\t{{dealer_id = {p.DealerId}, product_id = {p.ProductId}}}");
				Console.WriteLine("}\n");
			}
		}
	}
}
