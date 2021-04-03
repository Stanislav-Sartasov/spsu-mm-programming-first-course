using NUnit.Framework;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Task3Test
{
	public class Tests
	{

		[Test]
		public void MainTest()
		{
			HashSet<Task3.Product> constructedProducts = new HashSet<Task3.Product>();
			HashSet<Task3.Product> receivedProducts = new HashSet<Task3.Product>();
			int minReceivedProductCount = -1;
			int maxReceivedProductCount = -1;

			Task3.TestSystem test = new Task3.TestSystem(5, 5, 500);
			test.Start();
			Thread.Sleep(5000);
			test.Stop();

			foreach (var d in test.Dealers)
			{
				foreach (var p in d.LocalStorage)
				{
					Assert.IsFalse(constructedProducts.Contains(p));
					constructedProducts.Add(p);
				}
			}

			foreach (var c in test.Consumers)
			{
				if ((minReceivedProductCount == -1) || (minReceivedProductCount > c.LocalStorage.Count))
					minReceivedProductCount = c.LocalStorage.Count;
				if ((maxReceivedProductCount == -1) || (maxReceivedProductCount < c.LocalStorage.Count))
					maxReceivedProductCount = c.LocalStorage.Count;

				foreach (var p in c.LocalStorage)
				{
					Assert.IsFalse(receivedProducts.Contains(p));
					Assert.IsTrue(constructedProducts.Contains(p));
					receivedProducts.Add(p);
				}
			}

			Console.WriteLine($"number of dealers: {test.Dealers.Count}");
			Console.WriteLine($"number of consumers: {test.Consumers.Count}");
			Console.WriteLine($"number of products created: {constructedProducts.Count}");
			Console.WriteLine($"number of products received: {receivedProducts.Count}");
			Console.WriteLine($"number of products lost: {constructedProducts.Count - receivedProducts.Count}");
			Console.WriteLine($"minimum number of products received by one consumer: {minReceivedProductCount}");
			Console.WriteLine($"maximum number of products received by one consumer: {maxReceivedProductCount}");
			Console.WriteLine("\nlog:\n");
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