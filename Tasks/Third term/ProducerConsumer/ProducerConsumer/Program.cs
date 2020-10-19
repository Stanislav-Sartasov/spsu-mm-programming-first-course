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
            Manager<Object> manager = new Manager<object>();
            manager.Initialize(producer, consumer);
            manager.Run();
            Console.ReadKey();
            manager.Exit();
            manager.Dispose();
        }
    }
}
