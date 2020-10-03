using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FibersLib
{
    public static class ProcessManager
    {
        private static int currentPriority;
        private static int minPriority;
        private static bool isPriority;
        private static bool isStarted;
        private static readonly Random rnd;
        private static uint currentFiberID;
        private static List<uint> currentFibers;
        public static Dictionary<uint, Process> Processes { get; private set; }
        

        public static bool IsPriority
        {
            get
            {
                return isPriority;
            }
            set
            {
                if (!isStarted)
                    isPriority = value;
            }
        }

        static ProcessManager()
        {
            Processes = new Dictionary<uint, Process>();
            currentFibers = new List<uint>();
            rnd = new Random(); 
            isStarted = false;
            isPriority = false;
        }

        public static void AddProcess(Process process)
        {
            Fiber fiber = new Fiber(process.Run);
            bool added = false;
            while (!added)
            {
                if (!Processes.ContainsKey(fiber.Id))
                {
                    Processes.Add(fiber.Id, process);
                    added = true;
                }
                else
                {
                    fiber.Delete();
                    fiber = new Fiber(process.Run);
                }
            }
        }

        public static void Run()
        {
            var notFinishedProcessses = Processes.Where(p => p.Value.Status != ProcessStatus.Finished);
            if (notFinishedProcessses.Count() == 0 || isStarted)
            {
                return;
            }
            if (isPriority)
            {
                minPriority = notFinishedProcessses.Min(p => p.Value.Priority);
                currentPriority = notFinishedProcessses.Max(p => p.Value.Priority); 
                UpdateCurrentFibers();
            }
            isStarted = true;
            Switch(false);
        }

        public static void Dispose()
        {
            foreach (var process in Processes)
            {
                if (process.Key != Fiber.PrimaryId)
                {
                    Thread.Sleep(10);
                    Fiber.Delete(process.Key);
                }
            }
            isStarted = false;
            isPriority = false;
            currentFiberID = 0;
            currentPriority = 0;
            minPriority = 0;
            Processes.Clear();
            currentFibers.Clear();
        }

        internal static void Switch(bool isFinished)
        {
            if (isFinished)
            {
                Console.WriteLine("Fiber [{0}] Finished", currentFiberID);
            }

            if (Processes.Count(p => p.Value.Status != ProcessStatus.Finished) != 0)
            {
                currentFiberID = GetNextFiber();
            }
            else
            {
                currentFiberID = Fiber.PrimaryId;
            }

            Thread.Sleep(5);
            if (currentFiberID == Fiber.PrimaryId)
                Console.WriteLine($"Switched to the primary fiber...");
            Fiber.Switch(currentFiberID);
        }

        private static uint GetNextFiber()
        {
            if (IsPriority)
                return GetNextPriorityFiber();
            else
                return GetNextNonPriorityFiber();
        }

        private static uint GetNextNonPriorityFiber()
        {
            var selectedProcesses = Processes
                .Where(p => p.Value.Status != ProcessStatus.Finished 
                && p.Key != currentFiberID);
            if (selectedProcesses.Count() == 0)
            {
                return currentFiberID;
            }
            else
            {
                return selectedProcesses
                    .ElementAt(rnd.Next(selectedProcesses
                    .Count())).Key;
            }
        }

        private static void CalculateCurrentPriority()
        {
            var notFinishedProcessses = Processes
                .Where(p => p.Value.Status != ProcessStatus.Finished);
            minPriority = notFinishedProcessses.Min(p => p.Value.Priority);
            if (currentPriority <= minPriority)
                currentPriority = notFinishedProcessses.Max(p => p.Value.Priority);
            else
                currentPriority = notFinishedProcessses
                    .Where(p => p.Value.Priority < currentPriority)
                    .Max(p => p.Value.Priority);
        }

        private static void UpdateCurrentFibers()
        {
            currentFibers = Processes
                    .Where(p => p.Value.Status != ProcessStatus.Finished
                    && p.Value.Priority == currentPriority)
                    .Select(p => p.Key)
                    .ToList();
        }

        private static uint GetNextPriorityFiber()
        {
            if (currentFibers.Count() == 0)
            {
                CalculateCurrentPriority();
                UpdateCurrentFibers();
            }
            var nextFiberID = currentFibers.First();
            currentFibers.RemoveAt(0);
            return nextFiberID;
        }
    }
}
