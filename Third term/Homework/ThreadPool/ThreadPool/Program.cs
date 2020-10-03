using System;
using System.Threading;
using ThreadPool = ThreadPoolLib.ThreadPool;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ThreadPool pool = new ThreadPool())
            {
                for (int i = 0; i < 1000; i++)
                {
                    int num = i;
                    Thread.Sleep(1);
                    pool.Enqueue(() =>
                    {
                        Console.WriteLine(num);
                    });
                }
            }
            Console.WriteLine("Done");
        }
    }
}
