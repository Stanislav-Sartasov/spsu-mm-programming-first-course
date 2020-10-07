using System;
using System.Threading;

namespace ProducersConsumers
{
    public class Consumer : IDisposable
    {
        private ProductStorage storage;
        private Action receivedAction;
        private Thread handler;
        volatile bool isWorking;

        public int ConsumingInterval { get; }
     
        public Consumer(ProductStorage storage, int consumingInterval)
        {
            ConsumingInterval = consumingInterval;
            isWorking = true;
            this.storage = storage;
            handler = new Thread(StartConsuming);
        }

        private void Consume()
        {
            receivedAction?.Invoke();
        }

        private void TakeProduct()
        {
            receivedAction = storage.SendProduct();
        }

        private void StartConsuming()
        {
            while (isWorking)
            {
                TakeProduct();
                Consume();
                Thread.Sleep(ConsumingInterval);
            }
        }

        public void Run()
        {
            handler?.Start();
        }

        public void Dispose()
        {
            isWorking = false;
            handler.Join();
        }

    }
}
