using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProducersConsumers;

namespace ProducersConsumersTask
{
    class Program
    {
        static void Main(string[] args)
        {
            int consumersNumber = 2;
            int producersNumber = 4;
            int producingInterval = 500;
            int consumingInterval = 100;
            Console.WriteLine("Press any button to stop");
            using (var producerConsumerInteraction =
                new ProducerConsumerInteraction(consumersNumber, producersNumber,
                producingInterval, consumingInterval))
            {
                producerConsumerInteraction.Run();
                Console.ReadKey();
            }
            Console.WriteLine("Finished...");
        }
    }
}
