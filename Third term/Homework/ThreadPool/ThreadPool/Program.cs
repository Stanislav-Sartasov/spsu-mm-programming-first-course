using System;
using System.Threading;
using ThreadPool = ThreadPoolLib.ThreadPool;

namespace Task1
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
                    Thread.Sleep(1);
                    pool.Enqueue(() =>
                    {
                        while(true)
                            Console.WriteLine(num);
                    });
                }
            }   
        }
    }
}
