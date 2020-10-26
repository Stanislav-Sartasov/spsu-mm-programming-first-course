using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProdusersConsumers;

namespace TaskProducersConsumers.Tests
{
    [TestClass]
    public class TestProducersConsumers
    {
        [TestMethod]
        public void TestProducers()
        {
            List<int> products = new List<int>();
            List<Producer> producers = new List<Producer>();
            for (int i = 0; i < 3; i++)
            {
                producers.Add(new Producer(products));
            }
            Thread.Sleep(100);
            Assert.AreEqual(3, products.Count);
            foreach (var producer in producers)
                producer.Dispose();

        }
        [TestMethod]
        public void TestConsumers()
        {
            Random random = new Random();
            List<int> products = new List<int>();
            for (int i = 0; i < 3; i++)
                products.Add(random.Next(100));
            List<Consumer> consumers = new List<Consumer>();
            for (int i = 0; i < 3; i++)
            {
                consumers.Add(new Consumer(products));
            }
            Thread.Sleep(100);
            Assert.AreEqual(0, products.Count);
            foreach (var consumer in consumers)
                consumer.Dispose();
        }
    }
}
