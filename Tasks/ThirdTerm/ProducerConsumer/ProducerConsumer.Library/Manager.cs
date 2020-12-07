using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProducerConsumer.Library
{
    public class Manager<T> : IDisposable where T : class
    {
        private List<T> products;
        private List<Producer<T>> producers;
        private List<Consumer<T>> consumers;

        public void Initialize(List<Producer<T>> producers, List<Consumer<T>> consumers)
        {
            products = new List<T>();
            this.producers = producers;
            this.consumers = consumers;

            foreach (Producer<T> producer in producers)
                producer.Start();

            foreach (Consumer<T> consumer in consumers)
                consumer.Start();
        }

        public void Add(string name, T item)
        {
            TTASLock.Lock();
            products.Add(item);
            Console.WriteLine($"Producer {name} add the element {item}");
            TTASLock.Unlock();
        }

        public void Pop(string name)
        {
            TTASLock.Lock();
            if (products.Count != 0)
            {
                products.RemoveAt(products.Count - 1);
                Console.WriteLine($"Consumer {name} take a element");
            }
            TTASLock.Unlock();
        }


        public void Exit()
        {
            Dispose();
        }


        public void Dispose()
        {
            foreach(Producer<T> producer in producers)
                producer.Exit();

            foreach (Consumer<T> consumer in consumers)
                consumer.Exit();

            products.Clear();
        }
    }
}
