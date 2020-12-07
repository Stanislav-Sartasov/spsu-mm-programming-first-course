using System;
using System.Collections.Generic;
using System.Threading;
using ProducerConsumer.Library;

namespace ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
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
            Console.ReadKey();
            manager.Exit();
        }

    }
}
