using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            Library.ThreadPool threadPool = new Library.ThreadPool();
            threadPool.Start();
            for (int i = 0; i < 3; i++)
                threadPool.Enqueue(new Action(SomeAction));
            Thread.Sleep(2000);
            threadPool.Dispose();
        }

        private static void SomeAction()
        {
            Console.WriteLine("Starting to work");
            Thread.Sleep(new Random().Next(1, 1000));
            Console.WriteLine("finishing work\n");
        }
    }
}
