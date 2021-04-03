using System;
using System.Threading;
using System.Collections.Generic;

namespace Task4
{
	class Program
	{
		const int TasksCount = 30;
		static Random rand = new Random();
		static List<string> actionLog = new List<string>();
		static void Action()
		{
			Thread.Sleep(rand.Next(50, 1000));
			lock (actionLog)
				actionLog.Add($"Thread[{Thread.CurrentThread.Name}]\tfinish\tat {{{DateTime.Now}}}");
		}

		static void Main(string[] args)
		{
			ThreadPool threadPool = new ThreadPool();
			for (int i = 0; i < TasksCount; i++)
				threadPool.Enqueue(Action);

			Thread.Sleep(500);

			threadPool.Dispose();

			Console.WriteLine("ThreadPool log:");
			foreach (var s in threadPool.Log)
				Console.WriteLine(s);
			Console.WriteLine("\naction log:");
			foreach (var s in actionLog)
				Console.WriteLine(s);
		}
	}
}
