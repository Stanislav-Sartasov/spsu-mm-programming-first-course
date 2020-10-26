using System;
using System.Collections.Generic;
using ProdusersConsumers;

namespace TaskProducersConsumers
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> products = new List<int>();
            List<Producer> producers = new List<Producer>();
            List<Consumer> consumers = new List<Consumer>();
            for (int i = 0; i < 6; i++)
            {
                consumers.Add(new Consumer(products));
            }
            for (int i = 0; i < 3; i++)
            {
                producers.Add(new Producer(products));
            }
            Console.ReadKey();
            foreach (var consumer in consumers)
                consumer.Dispose();
            foreach (var producer in producers)
                producer.Dispose();
        }
    }
}
