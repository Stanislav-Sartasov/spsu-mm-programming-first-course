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

		private static Random rndmzer;

		public static void Switch(bool fiberFinished)
		{
			if (fiberFinished)
			{
				Console.WriteLine($"Fiber [{currFiber}] is finished");

				if (currFiber != Fiber.PrimaryId)
				{
					removedFibers.Add(currFiber);
				}
				Fibers.Remove(currFiber);
			}

			if (Fibers.Count == 0)
			{
				currFiber = Fiber.PrimaryId;
			}
			else if (IsPrioritized)
			{
				//currFiber = GetNextPriorFiber();
			}
			else
			{
				currFiber = Fibers.Keys.ElementAt<uint>(rndmzer.Next(Fibers.Keys.Count));
			}

			Thread.Sleep(1);

			if (currFiber == Fiber.PrimaryId)
			{
				Console.WriteLine("Switched to primary");
			}
			Fiber.Switch(currFiber);
		}

		public static void AddProcess()
		{
			var process = new Process();
			var fiber = new Fiber(process.Run);

			Fibers.Add(fiber.Id, process);
		}

		public static void Launch()
		{
			currFiber = Fibers.First().Key;
			if (IsPrioritized)
			{
				//init
			}

			Switch(false);
		}

		public static void Dispose()
		{
			foreach(var fiber in removedFibers)
			{
				Thread.Sleep(1);
				Fiber.Delete(fiber);
			}
			removedFibers.Clear();
			
			Fibers.Clear();

			IsPrioritized = default;
			currFiber = default;
			rndmzer = default;
		}

		private static uint GetNextPriorFiber()
		{
			/*
			var fiber = new Fiber();
			return fiber.Id;
			*/
			return default;
		}

		private static void UpdatePriorities()
		{

		}

		static ProcessManager()
		{
			Fibers = new Dictionary<uint, Process>();
			removedFibers = new List<uint>();

			rndmzer = new Random();
		}
	}
}
