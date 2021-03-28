using System;
using System.Threading;
using TPLib;

namespace FourthTask
{
	class Program
	{
		static void Main(string[] args)
		{
			const int numOfTasks = 20;
			var action = new Action(() => Console.WriteLine("Testing - testing 1, 2, 3!"));

			var tp = new TPLib.ThreadPool();
			Console.WriteLine("The program has started!");

			for (int i = 0; i < numOfTasks; i++)
			{
				tp.Enqueue(action);
				Thread.Sleep(20);
			}
			Thread.Sleep(20);

			tp.Dispose();
			Console.WriteLine("The program has finished!");
		}
	}
}