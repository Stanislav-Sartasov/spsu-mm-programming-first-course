using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProdusersConsumers
{
    public class Consumer : IDisposable
    {
        private Thread thread;
        private volatile bool isWorking;
        public Consumer(List<int> products)
        {
            isWorking = true;
            thread = new Thread(new ParameterizedThreadStart(GetProduct));
            thread.Start(products);
        }

        private void GetProduct(object obj)
        {
            List<int> products = (List<int>)obj;
            while (isWorking)
            {
                Monitor.Enter(products);
                if (products.Count > 0)
                {
                    Console.WriteLine($"Thread with id:{thread.ManagedThreadId} got product");
                    products.RemoveAt(0);
                }
                Monitor.Exit(products);
                Thread.Sleep(1000);
            }
        }

        public void Dispose()
        {
            isWorking = false;
            thread.Join();
        }
    }
}
