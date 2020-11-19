using System;
using System.Threading;

public class Program
{
    public static void Main()
    {
        ThreadPool.ThreadPool threadPool = new ThreadPool.ThreadPool();
        Action action = DoSomething;
        for (int i = 0; i < 100; i++)
        {
            threadPool.Enqueue(action);
            Thread.Sleep(100);
        }
        threadPool.Dispose();
        Console.WriteLine("Press any key.");
        Console.ReadKey();
    }

    private static void DoSomething()
    {
        if (int.Parse(Thread.CurrentThread.Name) % 2 == 0)
            Thread.Sleep(10000);
        else
            Thread.Sleep(1000);
        Console.WriteLine("Thread {0} fulfilled his task.", Thread.CurrentThread.Name);
        
    }
}
 
   