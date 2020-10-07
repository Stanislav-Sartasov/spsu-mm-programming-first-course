using System;
using System.Threading;

namespace ProducersConsumers
{
    public class Producer : IDisposable
    {
        private ProductStorage storage;
        private Action producedAction;
        private Thread handler;
        private int productsCounter;
        volatile bool isWorking;

        public int ProducingInterval { get; }

        Action<string> Output;

        public Producer(ProductStorage storage, string name, int producingInterval)
            : this(storage, name, producingInterval, s => Console.WriteLine(s), int.MinValue)
        {
        }

        public Producer(ProductStorage storage, string name, int producingInterval, 
            Action<string> output, int productsNumber)
        {
            productsCounter = productsNumber;
            ProducingInterval = producingInterval;
            isWorking = true;
            this.storage = storage;
            Output = output;
            handler = new Thread(StartProducing)
            {
                Name = name
            };
        }

        private void Produce()
        {
            if (productsCounter == int.MinValue) //means no upper border
                producedAction = () => Output("Produced at "
                    + DateTime.Now
                    + $" by {handler.Name}");
            else
            {
                if (productsCounter > 0)
                    producedAction = () => Output($"Task {productsCounter--} was produced by {handler.Name}");
                else
                    isWorking = false;
            }
        }

        private void SendProduct()
        {
            if (isWorking)
                storage.TakeProduct(producedAction);
        }

        private void StartProducing()
        {
            while (isWorking)
            {
                Produce();
                SendProduct();
                Thread.Sleep(ProducingInterval);
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
