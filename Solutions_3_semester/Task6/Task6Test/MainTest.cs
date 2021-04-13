using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Task6Test
{
	public class Tests
	{
		const int UserCount = 2000;
		const int TaskPerUser = 100;
		const int RandGen = 42;


		[Test]
		public void RefinableHashSetCorrectnessTest()
		{
			CorrectnessTest(new Task6.StaticSizeHashSet.StaticSizeHashSet(1 << 8));
		}

		[Test]
		public void LazySetCorrectnessTest()
		{
			CorrectnessTest(new Task6.LazySet.LazySet());
		}

		[Test]
		public void RefinableHashSetSpeedTest()
		{
			for (int k = 1; k <= 2; k++)
				SpeedTest(new Task6.StaticSizeHashSet.StaticSizeHashSet(1 << 8), UserCount, TaskPerUser * k);
		}

		[Test]
		public void LazySetSpeedTest()
		{
			for (int k = 1; k <= 2; k++)
				SpeedTest(new Task6.LazySet.LazySet(), UserCount, TaskPerUser * k);
		}

		[Test]
		public void SoloRefinableHashSetSpeedTest()
		{
			for (int k = 1; k <= 2; k++)
				SpeedTest(new Task6.StaticSizeHashSet.StaticSizeHashSet(1 << 8), 1, 20000 * k);
		}

		[Test]
		public void SoloLazySetSpeedTest()
		{
			for (int k = 1; k <= 2; k++)
				SpeedTest(new Task6.LazySet.LazySet(), 1, 5000 * k);
		}

		static void CorrectnessTest(Task6.IExamSystem table)
		{
			table.Add(1, 1);
			table.Add(1, 2);
			table.Add(2, 2);
			table.Add(3, 3);

			Assert.IsTrue(table.Contains(1, 1));
			Assert.IsTrue(table.Contains(1, 2));
			Assert.IsTrue(table.Contains(2, 2));
			Assert.IsTrue(table.Contains(3, 3));

			table.Remove(2, 2);

			Assert.IsTrue(table.Contains(1, 1));
			Assert.IsTrue(table.Contains(1, 2));
			Assert.IsFalse(table.Contains(2, 2));
			Assert.IsTrue(table.Contains(3, 3));

			table.Remove(1, 1);
			table.Remove(1, 2);
			table.Remove(2, 2);
			table.Remove(3, 3);

			Assert.IsFalse(table.Contains(1, 1));
			Assert.IsFalse(table.Contains(1, 2));
			Assert.IsFalse(table.Contains(2, 2));
			Assert.IsFalse(table.Contains(3, 3));
		}

		static void SpeedTest(Task6.IExamSystem table, int userCount, int taskPerUser)
		{
			var userTask = GetTasks(RandGen, userCount, taskPerUser, new int[] { 90, 9, 1 });

			bool start = false;

			List<Action<long, long>> actions = GetActions(table);

			List<Task> tasks = new List<Task>();
			for (int i = 0; i < userCount; i++)
			{
				List<int[]> task = userTask[i];
				tasks.Add(Task.Run(() =>
				{
					while (!start)
						Thread.Sleep(100);
					Work(task, actions);
				}));
			}

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			start = true;
			Task.WaitAll(tasks.ToArray());
			stopwatch.Stop();
			Console.WriteLine($"completed in {stopwatch.ElapsedMilliseconds}ms with {userCount} users and {taskPerUser} tasks per user");
		}

		static List<List<int[]>> GetTasks(int randGen, int userCount, int taskCountPerUser, int[] probabilityOfEvent)
		{
			Random rand = new Random(randGen);

			List<List<int[]>> result = new List<List<int[]>>();

			int sumProb = 0;
			foreach (var p in probabilityOfEvent)
				sumProb += p;

			for (int i = 0; i < userCount; i++)
			{
				List<int[]> task = new List<int[]>();
				for (int j = 0; j < taskCountPerUser; j++)
				{
					int taskId = rand.Next(0, sumProb);
					for (int p = 0; p < probabilityOfEvent.Length; p++)
						if (taskId >= probabilityOfEvent[p])
							taskId -= probabilityOfEvent[p];
						else
						{
							taskId = p;
							break;
						}
					task.Add(new int[] { taskId, rand.Next(0, int.MaxValue), rand.Next(0, int.MaxValue) });
				}
				result.Add(task);
			}

			return result;
		}

		static List<Action<long, long>> GetActions(Task6.IExamSystem table)
		{
			return new List<Action<long, long>>()
			{
				(x, y) => { table.Contains(x, y); },
				table.Add,
				table.Remove
			};
		}

		static void Work(List<int[]> tasks, List<Action<long, long>> actions)
		{
			foreach (var task in tasks)
				actions[task[0]](task[1], task[2]);
		}
	}
}