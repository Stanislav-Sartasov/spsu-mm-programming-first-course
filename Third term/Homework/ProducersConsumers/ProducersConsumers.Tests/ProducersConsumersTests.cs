using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProducersConsumers.Tests
{
    [TestClass]
    public class ProducersConsumersTests
    {
        List<string> logs;
        List<Consumer> consumers;
        List<Producer> producers;
        ProductStorage storage;
        int productsNumber = 10;
        int consumersNumber = 2;
        int producersNumber = 4;
        int producingInterval = 200;
        int consumingInterval = 50;

        [TestInitialize]
        public void Init()
        {
            storage = new ProductStorage();
            logs = new List<string>();
            consumers = Enumerable.Range(0, consumersNumber)
                .Select(n => new Consumer(storage, consumingInterval))
                .ToList();
            producers = Enumerable.Range(0, producersNumber)
                .Select(n => new Producer(storage, n.ToString(), producingInterval, s => logs.Add(s), productsNumber))
                .ToList();
            consumers.ForEach(c => c.Run());
            producers.ForEach(c => c.Run());
            Thread.Sleep(1000);
            while (logs.Count() != productsNumber * producersNumber)
                Thread.Sleep(1000);
            consumers.ForEach(c => c.Dispose());
            producers.ForEach(c => c.Dispose());

        }


        [TestMethod]
        public void CorrectTasksNumber()
        {
            for (int producer = 0; producer < producersNumber; producer++)
                for (int task = 1; task <= productsNumber; task++)
                    Assert.IsTrue(logs.Contains($"Task {task} was produced by {producer}"));
            Assert.AreEqual(productsNumber * producersNumber, logs.Count());
        }
    }
}
