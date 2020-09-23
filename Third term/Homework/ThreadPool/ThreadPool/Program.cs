using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pool = new ThreadPool())
            {
                for (int i = 0; i < 1000; i++)
                {
                    int num = i;
                    pool.Enqueue(() =>
                    {
                        Console.WriteLine(num);
                        Thread.Sleep(10);
                    });
                }
            }
            Console.WriteLine("Done");
        }
    }
}
