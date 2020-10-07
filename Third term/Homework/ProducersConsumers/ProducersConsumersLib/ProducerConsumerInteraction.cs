using System;
using System.Collections.Generic;
using System.Linq;

namespace ProducersConsumers
{
    public class ProducerConsumerInteraction : IDisposable
    {
        private ProductStorage storage;
        private List<Consumer> consumers;
        private List<Producer> producers;
        private bool isDisposed;

        public ProducerConsumerInteraction(int consumersNumber, int producersNumber,
            int producingInterval, int consumingInterval)
        {
            isDisposed = false;
            storage = new ProductStorage();
            consumers = Enumerable.Range(0, consumersNumber)
                .Select(n => new Consumer(storage, consumingInterval))
                .ToList();
            producers = Enumerable.Range(0, producersNumber)
                .Select(n => new Producer(storage, n.ToString(), producingInterval))
                .ToList();
        }

        public void Dispose()
        {
            if (isDisposed)
                return;
            consumers.ForEach(c => c.Dispose());
            producers.ForEach(c => c.Dispose());
            isDisposed = true;
        }

        public void Run()
        {
            if (!isDisposed)
            {
                consumers.ForEach(c => c.Run());
                producers.ForEach(c => c.Run());
            }
            else
                throw new InvalidOperationException("ProducerConsumerInteraction has been disposed");
        }
    }
}
