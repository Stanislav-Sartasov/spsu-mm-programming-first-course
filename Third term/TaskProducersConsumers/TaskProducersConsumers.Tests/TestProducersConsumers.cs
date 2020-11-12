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
        public void TestManager()
        {
            Manager manager = new Manager();
            List<Producer> producers = new List<Producer>();
            List<Consumer> consumers = new List<Consumer>();
            for (int i = 0; i < 6; i++)
            {
                consumers.Add(new Consumer(manager));
            }
            for (int i = 0; i < 3; i++)
            {
                producers.Add(new Producer(manager));
            }
            manager.Initialize(consumers, producers);
            Thread.Sleep(100);
            Assert.AreEqual(3, manager.GetCount());
            manager.Dispose();
            Assert.AreEqual(0, manager.GetCount());

        }
        
    }
}
