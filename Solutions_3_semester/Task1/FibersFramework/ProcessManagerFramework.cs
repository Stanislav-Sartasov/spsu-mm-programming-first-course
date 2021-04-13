using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fibers.Framework;

namespace Processes
{
    public static class ProcessManager
    {
        static Dictionary<uint, int> taskPriorities = new Dictionary<uint, int>();
        static Dictionary<uint, Process> tasks = new Dictionary<uint, Process>();
        static int prioritiesSum = 0;
        static List<uint> removed = new List<uint>();
        static int processCount = 0;
        static uint actualFiberId = 0;
        static Random rnd = new Random();
        public static bool Priority { get; set; } = false;
        public static bool Init(Process[] processes)
        {
            try
            {
                processCount = processes.Length;
                foreach (var process in processes)
                {
                    Fiber fiber = new Fiber(process.Run);
                    actualFiberId = fiber.Id;
                    tasks[actualFiberId] = process;
                    taskPriorities[actualFiberId] = process.Priority;
                    prioritiesSum += process.Priority;
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
                RemoveFiber(actualFiberId);

            uint next = GetNextFiberId();

            if (actualFiberId != next)
            {
                actualFiberId = next;
                Fiber.Switch(actualFiberId);
            }
        }
        static void RemoveFiber(uint fiberId)
        {
            tasks.Remove(fiberId);

            prioritiesSum -= taskPriorities[fiberId];
            taskPriorities.Remove(fiberId);

            removed.Add(fiberId);
            processCount--;
        }
        static uint GetNextFiberId()
        {
            if (processCount == 0)
                return Fiber.PrimaryId;

            if (Priority)
            {
                int rand = rnd.Next(prioritiesSum);
                uint result = taskPriorities.Keys.ElementAt(0);
                foreach (uint fiberId in taskPriorities.Keys)
                {
                    result = fiberId;
                    if (rand < taskPriorities[fiberId])
                        break;
                    else
                        rand -= taskPriorities[fiberId];
                }
                return result;
            }
            else
                return taskPriorities.Keys.ElementAt(rnd.Next(taskPriorities.Keys.Count));
        }
        public static void SwitchPriority()
        {
            Priority = !Priority;
        }
        public static void SwitchPriority(bool newPriority)
        {
            Priority = newPriority;
        }

        public static void Dispose()
        {
            foreach (var key in removed)
                Fiber.Delete(key);

            removed.Clear();
            taskPriorities.Clear();
            tasks.Clear();
            actualFiberId = 0;
            processCount = 0;
            prioritiesSum = 0;
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