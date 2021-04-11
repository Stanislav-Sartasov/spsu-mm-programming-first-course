using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using HTLib;

namespace SixthTask
{
	class Program
	{
		static IExamSystem deanery;
		static List<Task> tasks;
		static int tasksNum = 100000;

		static void Main()
		{
			var closedTable = new RefinableHT(1024);
			var cuckooTable = new StripedCuckooHT(1024);

			var stopwatch = new Stopwatch();
			tasks = new List<Task>(tasksNum);

			TasksInit();
			deanery = closedTable;
			stopwatch.Start();
			StartTest();
			stopwatch.Stop();
			Console.WriteLine($"Closed table running time after {tasksNum} requests: {stopwatch.ElapsedMilliseconds} ms");
			tasks.Clear();

			TasksInit();
			deanery = cuckooTable;
			stopwatch.Restart();
			StartTest();
			stopwatch.Stop();
			Console.WriteLine($"Cuckoo table running time after {tasksNum} requests: {stopwatch.ElapsedMilliseconds} ms");
			tasks.Clear();
		}

		public static void TasksInit()
		{
			var rnd = new Random();
			for (int i = 0; i < tasksNum; i++)
			{
				int chs = rnd.Next(100);

				if (chs < 90)
				{
					tasks.Add(new Task(() => deanery.Contains(rnd.Next(), rnd.Next())));
				}
				else if (chs < 99)
				{
					tasks.Add(new Task(() => deanery.Add(rnd.Next(), rnd.Next())));

				}
				else
				{
					tasks.Add(new Task(() => deanery.Remove(rnd.Next(), rnd.Next())));

				}
			}
		}

		public static void StartTest()
		{
			foreach (var task in tasks)
			{
				task.Start();
			}
			foreach (var task in tasks)
			{
				task.Wait();
			}
		}
	}
}