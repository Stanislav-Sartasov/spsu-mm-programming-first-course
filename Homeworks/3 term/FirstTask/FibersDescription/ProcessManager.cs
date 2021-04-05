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

		private static uint GetNextPriorFiber() //probability scheme
		{
			int rnd = rndmzer.Next(20); //1/20 probability that we take last fiber in dictionary 
			uint priorFiber = default;

			if (rnd == 0)
			{
				priorFiber = Fibers.Last().Key;
			}
			else
			{
				int max = fibersPriorities.Max(x => x.Value);
				var temp = fibersPriorities.Where(x => (x.Value == max)).ToDictionary(x => x.Key);

				foreach (var chs in temp)
				{
					var chsRnd = rndmzer.Next(2); //1/2 chance for each fiber with max priority

					if (chsRnd == 1)
					{
						priorFiber = chs.Key;
						break;
					}
				}
				priorFiber = temp.Last().Key; //if we didn't choose last fiver with max priority in foreach
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

			//IsPrioritized = default;
			//rndmzer = null;
		}

		static ProcessManager()
		{
			Fibers = new Dictionary<uint, Process>();

			removedFibers = new List<uint>();
			currFiber = default;

			fibersPriorities = new Dictionary<uint, int>();

			rndmzer = new Random();
		}
	}
}
