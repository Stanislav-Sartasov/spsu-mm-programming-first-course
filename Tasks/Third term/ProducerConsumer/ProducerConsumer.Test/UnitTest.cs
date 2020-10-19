using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProducerConsumer.Library;

using System;
using System.Threading;

namespace ProducerConsumer.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test()
        {
            const int producer = 2;
            const int consumer = 2;
            Manager<Object> manager = new Manager<object>();
            manager.Initialize(producer, consumer);
            manager.Run();
            Thread.Sleep(100);
            manager.Exit();
            var producers = manager.GetProducers();
            var consumers = manager.GetConsumers();
            for (int i = 0; i < 2; i++)
            {
                Assert.IsFalse(producers[i].IsRunning());
                Assert.IsFalse(consumers[i].IsRunning());
            }
            manager.Dispose();
        }
    }
}
