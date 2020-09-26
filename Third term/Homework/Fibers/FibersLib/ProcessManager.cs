using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fibers
{
    public static class ProcessManager
    {
        public static bool IsPriority { get; set; }
        private static bool isStarted;
        public static Dictionary<uint, Process> Processes { get; private set; }
        private static readonly Random rnd = new Random();
        private static int numberOfProc;
        private static uint currentFiberId;

        static ProcessManager()
        {
            Processes = new Dictionary<uint, Process>();
            isStarted = false;
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
            numberOfProc = Processes.Count();
            if (numberOfProc == 0 || isStarted)
            {
                return;
            }
            isStarted = true;
            currentFiberId = Processes.Last().Key;
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
            Processes.Clear();
        }

        public static void Switch(bool isFinished)
        {
            if (isFinished)
            {
                Console.WriteLine("Fiber [{0}] Finished", currentFiberId);
            }

            if (Processes.Count(f => !f.Value.IsFinished) != 0)
            {
                currentFiberId = GetNextFiber();
            }
            else
            {
                currentFiberId = Fiber.PrimaryId;
            }

            Thread.Sleep(5);
            Fiber.Switch(currentFiberId);
        }

        public static uint GetNextFiber()
        {
            var workingProcesses = Processes.Where(p => !p.Value.IsFinished && p.Key != currentFiberId);
            if (workingProcesses.Count() == 0)
            {
                return currentFiberId;
            }
            else
            {
                if (IsPriority)
                {
                    return workingProcesses.OrderByDescending(p => p.Value.Priority).First().Key;
                }
                return workingProcesses.ElementAt(rnd.Next(workingProcesses.Count())).Key;
            }
        }
    }
}
