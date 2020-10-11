using System;
using ProducerConsumer.Library;

namespace ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int producer = 5;
            const int consumer = 5;
            Manager<Object>.Initialize(producer, consumer);
            Manager<Object>.Run();
            Console.ReadKey();
            Manager<Object>.Exit();
            Manager<Object>.Dispose();
        }
    }
}
