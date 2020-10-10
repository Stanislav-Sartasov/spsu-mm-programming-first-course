using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fibers.Framework
{
    public static class ProcessManager
    {
        private static uint currentFiber;
        private static Dictionary<uint, Process> tasks;
        private static List<uint> delList;
        private static bool isPriority;
        private static List<uint> tempFibers;
        private static int count;

        public static bool Initialize(List<Process> processes, bool priority)
        {
            bool success = false;
            try
            {
                isPriority = priority;
                tasks = new Dictionary<uint, Process>();
                tempFibers = new List<uint>();
                delList = new List<uint>();
                count = processes.Count;

                foreach (var process in processes)
                {
                    Fiber fiber = new Fiber(process.Run);
                    tasks[fiber.Id] = process;
                    currentFiber = fiber.Id;
                }
                success = true;
            }
            catch
            {
                success = false;
            }
            return success;
        }
        public static bool Initialize(List<Process> processes)
        {
            return Initialize(processes, false);
        }
        public static void Run()
        {
            Switch(false);
        }
        internal static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                Console.WriteLine($"Fiber {currentFiber} finished");
                //tasks.Remove(currentFiber);
                delList.Add(currentFiber);
                //if (currentFiber != Fiber.PrimaryId)
                //    Fiber.Delete(currentFiber);
            }
            currentFiber = GetNextProcess();
            while (delList.Contains(currentFiber))
                currentFiber = GetNextProcess();
            if (currentFiber == Fiber.PrimaryId)
                Console.WriteLine("Go to primary fiber");
            Fiber.Switch(currentFiber);
        }

        private static uint GetNextProcess()
        {
            uint id = 0;
            if (delList.Count == count)
                id = Fiber.PrimaryId;
            else if (isPriority)
            {
                if (tempFibers.Count == 0)
                    tempFibers = GetNextPriorityFibers();
                id = tempFibers[0];
                tempFibers.RemoveAt(0);
            }
            else
            {
                var keys = tasks.Keys;
                Random random = new Random();
                id = keys.ElementAt<uint>(random.Next(keys.Count));
            }
            return id;
        }

        private static List<uint> GetNextPriorityFibers()
        {
            int currentPriority = tasks[currentFiber].Priority;
            List<uint> result;
            int nextPriority = -1;
            int maxPriority = 0;
            foreach (var process in tasks.Values)
            {
                if (process.Priority > maxPriority)
                    maxPriority = process.Priority;
                if (nextPriority < process.Priority && process.Priority < currentPriority)
                    nextPriority = process.Priority;
            }
            if (nextPriority != -1)
                result = GetFibersWithDefinitePriority(nextPriority);
            else
                result = GetFibersWithDefinitePriority(maxPriority);
            return result;
        }

        private static List<uint> GetFibersWithDefinitePriority(int priority)
        {
            List<uint> result = new List<uint>();
            foreach (var pair in tasks)
            {
                if (pair.Value.Priority == priority)
                {
                    result.Add(pair.Key);
                }
            }
            return result;
        }

        public static void Dispose()
        {
            foreach (uint key in delList)
                Fiber.Delete(key);
            isPriority = default;
            tasks = null;
            tempFibers = null;
            delList = null;
            count = default;
        }

        // for tests
        public static List<uint> GetDelList()
        {
            return delList;
        }
    }
}
