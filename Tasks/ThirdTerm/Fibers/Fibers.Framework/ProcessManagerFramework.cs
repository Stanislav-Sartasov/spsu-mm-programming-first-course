using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fibers.Framework;

namespace Processes
{
    public static class ProcessManager
    {
        private static uint currentFiber;
        private static List<uint> removedFiberList;
        private static Dictionary<uint, Process> tasks;
        private static int countOfProcesses;
        private static Random rng;
        private static bool priority;
        private static List<KeyValuePair<uint, int>> priorities;
        private static int sumOfPriorities;

        static ProcessManager()
        {
            removedFiberList = new List<uint>();
            tasks = new Dictionary<uint, Process>();
            rng = new Random();
            priority = false;
            priorities = new List<KeyValuePair<uint, int>>();
            sumOfPriorities = 0;
        }

        public static bool InitializeProcesses(List<Process> processes)
        {
            try
            {
                countOfProcesses = processes.Count;
                foreach (Process process in processes)
                {
                    Fiber fiber = new Fiber(process.Run);
                    tasks[fiber.Id] = process;
                    currentFiber = fiber.Id;
                    priorities.Add(new KeyValuePair<uint, int>(currentFiber, process.Priority));
                    sumOfPriorities += process.Priority;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Run()
        {
            Switch(false);
        }

        public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                Console.WriteLine($"Fiber [{currentFiber}] Finished.");
                Remove(currentFiber);
            }

            uint nextFiber;
            nextFiber = GetNextProcess();

            if (nextFiber == Fiber.PrimaryId)
            {
                Console.WriteLine($"Go to [{currentFiber}] (primary) fiber.");
            }

            if (currentFiber != nextFiber)
            {
                currentFiber = nextFiber;
                Fiber.Switch(currentFiber);
            }
        }

        private static uint GetNextProcess()
        {
            if (countOfProcesses == 0)
            {
                return Fiber.PrimaryId;
            }

            if (priority)
            {
                return GetNextProcessWithPriority();
            }
            else
            {
                return tasks.Keys.ElementAt<uint>(rng.Next(tasks.Keys.Count));
            }
        }

        private static uint GetNextProcessWithPriority()
        {
            int randomSelection = rng.Next(sumOfPriorities + 1);
            foreach (KeyValuePair<uint, int> priority in priorities)
            {
                randomSelection -= priority.Value;
                if (randomSelection <= 0)
                {
                    return priority.Key;
                }
            }
            return priorities[priorities.Count - 1].Key;
        }

        private static void Remove(uint fiber)
        {
            tasks.Remove(fiber);
            removedFiberList.Add(fiber);

            foreach (KeyValuePair<uint, int> priority in priorities)
            {
                if (priority.Key == fiber)
                {
                    priorities.Remove(priority);
                    sumOfPriorities -= priority.Value;
                    break;
                }
            }

            countOfProcesses--;
        }

        public static void SwitchPriority()
        {
            priority = !priority;
        }

        public static void SwitchPriority(bool newPriority)
        {
            priority = newPriority;
        }

        public static void Dispose()
        {
            currentFiber = default;

            foreach (uint key in removedFiberList)
            {
                Fiber.Delete(key);
            }
            removedFiberList = null;

            tasks = null;
            countOfProcesses = default;
            rng = null;
            priority = default;
            priorities.Clear();
            priorities = null;
            sumOfPriorities = default;
        }
    }

    public class Process
    {
        private static readonly Random Rng = new Random();
        private const int LongPauseBoundary = 10000;
        private const int ShortPauseBoundary = 100;
        private const int WorkBoundary = 1000;
        private const int IntervalsAmountBoundary = 10;
        private const int PriorityLevelsNumber = 10;
        private readonly List<int> _workIntervals = new List<int>();
        private readonly List<int> _pauseIntervals = new List<int>();
        public Process()
        {
            int amount = Rng.Next(IntervalsAmountBoundary);
            for (int i = 0; i < amount; i++)
            {
                _workIntervals.Add(Rng.Next(WorkBoundary));
                _pauseIntervals.Add(Rng.Next(
                Rng.NextDouble() > 0.9
                ? LongPauseBoundary
                : ShortPauseBoundary));
            }
            Priority = Rng.Next(PriorityLevelsNumber);
        }
        public void Run()
        {
            for (int i = 0; i < _workIntervals.Count; i++)
            {
                Thread.Sleep(_workIntervals[i]); // work emulation
                DateTime pauseBeginTime = DateTime.Now;
                do
                {
                    ProcessManager.Switch(false);
                } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < _pauseIntervals[i]); // I/O emulation
            }
            ProcessManager.Switch(true);
        }
        public int Priority
        {
            get; private set;
        }
        public int TotalDuration
        {
            get
            {
                return ActiveDuration + _pauseIntervals.Sum();
            }
        }
        public int ActiveDuration
        {
            get
            {
                return _workIntervals.Sum();
            }
        }
    }
}