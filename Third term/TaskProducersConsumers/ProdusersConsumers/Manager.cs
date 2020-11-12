using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProdusersConsumers
{
    public class Manager : IDisposable
    {
        private List<int> products;
        private List<Consumer> consumers;
        private List<Producer> producers;
        public void Initialize(List<Consumer> consumers, List<Producer> producers)
        {
            products = new List<int>();
            this.consumers = consumers;
            this.producers = producers;
            foreach (var consumer in consumers)
                consumer.Start();
            foreach (var producer in producers)
                producer.Start();
        }

        public void SetProduct(Thread thread, int product)
        {
            Monitor.Enter(products);
            Console.WriteLine($"Thread with id:{thread.ManagedThreadId} added new product");
            products.Add(product);
            Monitor.Exit(products);
        }
        public void GetProduct(Thread thread)
        {
            Monitor.Enter(products);
            if (products.Count > 0)
            {
                foreach (var x in products)
                    Console.WriteLine(x);
                Console.WriteLine($"Thread with id:{thread.ManagedThreadId} got product");
                products.RemoveAt(0);
            }
            Monitor.Exit(products);
        }

        public int GetCount()
        {
            return products.Count;
        }
        public void Dispose()
        {
            foreach (var consumer in consumers)
                consumer.Dispose();
            foreach (var producer in producers)
                producer.Dispose();
            products.Clear();
        }
    }
}
