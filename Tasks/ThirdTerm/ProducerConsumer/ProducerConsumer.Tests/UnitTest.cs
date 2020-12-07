using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProducerConsumer.Library;

namespace ProducerConsumer.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test()
        {
            Manager<string> manager = new Manager<string>();

            const int countOfProducer = 3;
            const int countOfConsumer = 4;

            List<Producer<string>> producers = new List<Producer<string>>();
            List<Consumer<string>> consumers = new List<Consumer<string>>();

            for (int i = 0; i < countOfProducer; i++)
            {
                producers.Add(new Producer<string>(manager, i.ToString()));
            }
            for (int i = 0; i < countOfConsumer; i++)
            {
                consumers.Add(new Consumer<string>(manager, i.ToString()));
            }

            manager.Initialize(producers, consumers);

            foreach (Producer<string> producer in producers)
            {
                Assert.IsTrue(producer.IsWorking());
            }

            foreach (Consumer<string> consumer in consumers)
            {
                Assert.IsTrue(consumer.IsWorking());
            }

            manager.Exit();

            foreach (Producer<string> producer in producers)
            {
                Assert.IsFalse(producer.IsWorking());
            }

            foreach (Consumer<string> consumer in consumers)
            {
                Assert.IsFalse(consumer.IsWorking());
            }
        }
    }
}
