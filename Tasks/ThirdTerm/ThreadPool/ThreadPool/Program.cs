using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        private static void SomeWork()
        {
            Random random = new Random();
            int randomNumber = random.Next(10000);
            Thread.Sleep(randomNumber);
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} did its job in {randomNumber} milliseconds.");
        }

        public static void Main()
        {
            ThreadPool threadPool = new ThreadPool();
            Action action = SomeWork;
            for (int i = 0; i < 25; i++)
            {
                threadPool.Enqueue(action);
                Thread.Sleep(100);
            }
            threadPool.Dispose();
            Console.WriteLine("The tasks are over.");
        }
    }
}
