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
            Manager<Object>.Initialize(producer, consumer);
            Manager<Object>.Run();
            Thread.Sleep(100);
            Manager<Object>.Exit();
            var producers = Manager<Object>.GetProducers();
            var consumers = Manager<Object>.GetConsumers();
            for (int i = 0; i < 2; i++)
            {
                Assert.IsFalse(producers[i].continueRun);
                Assert.IsFalse(consumers[i].continueRun);
            }
            Manager<Object>.Dispose();
        }
    }
}
