using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer.Library
{
    public class Manager<T> where T : class
    {
        private List<Producer<T>> producers;
        private List<Consumer<T>> consumers;
        private List<T> buffer;
        public void Initialize(int producer, int consumer)
        {
            producers = new List<Producer<T>>(producer);
            consumers = new List<Consumer<T>>(consumer);
            buffer = new List<T>();

            for (int i = 0; i < producer; i++)
                producers.Add(new Producer<T>($"Produser {i + 1}", this));

            for (int i = 0; i < consumer; i++)
                consumers.Add(new Consumer<T>($"Consumer {i + 1}", this));
        }

        internal void Put(string name, T t)
        {
            Monitor.Enter(buffer);
            buffer.Add(t);
            Console.WriteLine($"Producer {name} add the element {t}");
            Monitor.Exit(buffer);
        }

        internal void Take(string name)
        {
            Random random = new Random();
            Monitor.Enter(buffer);
            if (buffer.Count != 0)
            {
                Console.WriteLine($"Consumer {name} take a element");
                buffer.RemoveAt(random.Next(buffer.Count));
            }
            Monitor.Exit(buffer);
        }

        public void Run()
        {
            for (int i = 0; i < producers.Count; i++)
                producers[i].Start();

            for (int i = 0; i < consumers.Count; i++)
                consumers[i].Start();
        }

        public List<Producer<T>> GetProducers()
        {
            return producers;
        }

        public List<Consumer<T>> GetConsumers()
        {
            return consumers;
        }

        public void Exit()
        {
            for (int i = 0; i < producers.Count; i++)
                producers[i].Exit();

            for (int i = 0; i < consumers.Count; i++)
                consumers[i].Exit();
        }

        public void Dispose()
        {
            producers = null;
            consumers = null;
            buffer = null;
        }
    }
}
