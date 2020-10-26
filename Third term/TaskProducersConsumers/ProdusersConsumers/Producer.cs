using System;
using System.Collections.Generic;
using System.Threading;

namespace ProdusersConsumers
{
    
    public class Producer : IDisposable
    {
        private Thread thread;
        private volatile bool isWorking;
        private Random random = new Random();

        public Producer(List<int> products)
        {
            isWorking = true;
            thread = new Thread(new ParameterizedThreadStart(SetProduct));
            thread.Start(products);
        }

        public void Dispose()
        {
            isWorking = false;
            thread.Join();
        }

        private void SetProduct(object obj)
        {
            List<int> products = (List<int>)obj;
            while (isWorking)
            {
                Monitor.Enter(products);
                Console.WriteLine($"Thread with id:{thread.ManagedThreadId} added new product");
                products.Add(random.Next(100));
                Monitor.Exit(products);
                Thread.Sleep(1000);
            }
        }
    }
}
