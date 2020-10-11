using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer.Library
{
    public static class Manager<T>  where T : class
    {
        private static List<Producer<T>> producers;
        private static List<Consumer<T>> consumers;
        private static List<T> Buffer;
        public static void Initialize(int producer, int consumer)
        {
            producers = new List<Producer<T>>(producer);
            consumers = new List<Consumer<T>>(consumer);
            Buffer = new List<T>();

            for (int i = 0; i < producer; i++)
                producers.Add(new Producer<T>($"Produser {i + 1}"));

            for (int i = 0; i < consumer; i++)
                consumers.Add(new Consumer<T>($"Consumer {i + 1}"));
        }

        internal static void Put(string name, T t)
        {
            Monitor.Enter(Buffer);
            Monitor.PulseAll(Buffer);
            Buffer.Add(t);
            Console.WriteLine($"Producer {name} add the element {t}");
            Monitor.PulseAll(Buffer);
            Monitor.Exit(Buffer);
        }

        internal static void Take(string name)
        {
            Random random = new Random();
            Monitor.Enter(Buffer);
            Monitor.PulseAll(Buffer);
            if (Buffer.Count != 0)
            {
                Console.WriteLine($"Consumer {name} take a element");
                Buffer.RemoveAt(random.Next(Buffer.Count));
            }
            Monitor.PulseAll(Buffer);
            Monitor.Exit(Buffer);
        }

        public static void Run()
        {
            for (int i = 0; i < producers.Count; i++)
                producers[i].Start();

            for (int i = 0; i < consumers.Count; i++)
                consumers[i].Start();
        }

        public static List<Producer<T>> GetProducers()
        {
            return producers;
        }

        public static List<Consumer<T>> GetConsumers()
        {
            return consumers;
        }

        public static void Exit()
        {
            for (int i = 0; i < producers.Count; i++)
                producers[i].Exit();

            for (int i = 0; i < consumers.Count; i++)
                consumers[i].Exit();
        }

        public static void Dispose()
        {
            producers = null;
            consumers = null;
            Buffer = null;
        }
    }
}
