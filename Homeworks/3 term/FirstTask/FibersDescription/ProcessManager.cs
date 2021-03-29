using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace FibersDescription
{
	public static class ProcessManager
	{
		public static bool IsPrioritized { get; set; }
		public static Dictionary<uint, Process> Fibers { get; private set; }

		private static List<uint> removedFibers;
		private static uint currFiber;

		private static Dictionary<uint, int> fibersPriorities;
		private static int sumPriority;

		private static Random rndmzer;

		public static void AddProcess()
		{
			var process = new Process();
			var fiber = new Fiber(process.Run);

			Fibers.Add(fiber.Id, process);
		}

		public static void Launch()
		{
			if (IsPrioritized)
			{
				foreach (var fiber in Fibers)
				{
					fibersPriorities.Add(fiber.Key, fiber.Value.Priority);
					sumPriority += fiber.Value.Priority;
				}
				currFiber = Fibers.OrderByDescending(x => x.Value.Priority).First().Key;
			}
			else
			{
				currFiber = Fibers.First().Key;
			}

			Switch(false);
		}

		public static void Switch(bool fiberFinished)
		{
			if (fiberFinished)
			{
				Console.WriteLine($"Fiber [{currFiber}] is finished.");

				if (currFiber != Fiber.PrimaryId)
				{
					removedFibers.Add(currFiber);
				}

				if (IsPrioritized)
				{
					sumPriority -= fibersPriorities[currFiber];
					fibersPriorities.Remove(currFiber);
				}

				Fibers.Remove(currFiber);
			}

			if (Fibers.Count == 0)
			{
				currFiber = Fiber.PrimaryId;
			}
			else if (IsPrioritized)
			{
				currFiber = GetNextPriorFiber();
			}
			else
			{
				currFiber = Fibers.Keys.ElementAt(rndmzer.Next(Fibers.Keys.Count));
			}

			Thread.Sleep(1);

			if (currFiber == Fiber.PrimaryId)
			{
				Console.WriteLine("Switched to primary.");
			}
			Fiber.Switch(currFiber);
		}

		private static uint GetNextPriorFiber()
		{
			int rndSum = rndmzer.Next(sumPriority + 1);
			uint priorFiber = default;

			foreach (var chs in fibersPriorities)
			{
				rndSum -= chs.Value;
				priorFiber = chs.Key;

				if (rndSum <= 0)
				{
					break;
				}
			}

			return priorFiber;
		}

		public static void Dispose()
		{
			foreach (var fiber in removedFibers)
			{
				Thread.Sleep(1);
				Fiber.Delete(fiber);
			}
			removedFibers.Clear();
			currFiber = default;

			Fibers.Clear();

			fibersPriorities.Clear();
			sumPriority = default;

			//IsPrioritized = default;
			//rndmzer = null;
		}

		static ProcessManager()
		{
			Fibers = new Dictionary<uint, Process>();

			removedFibers = new List<uint>();
			currFiber = default;

			fibersPriorities = new Dictionary<uint, int>();
			sumPriority = default;

			rndmzer = new Random();
		}
	}
}
