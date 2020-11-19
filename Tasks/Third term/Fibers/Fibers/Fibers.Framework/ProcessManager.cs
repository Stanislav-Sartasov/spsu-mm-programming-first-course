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
        private static Random random;
        private static List<int> priorities;

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
                random = new Random();
                priorities = new List<int>(10);

                foreach (var process in processes)
                {
                    Fiber fiber = new Fiber(process.Run);
                    tasks[fiber.Id] = process;
                    currentFiber = fiber.Id;
                    if (!priorities.Contains(process.Priority + 1))
                    {
                        priorities.Add(process.Priority + 1); // 0 is bad
                    }
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
                id = GetPriorityFiber();
            }
            else
            {
                var keys = tasks.Keys;
                id = keys.ElementAt<uint>(random.Next(keys.Count));
            }
            return id;
        }

        private static uint GetPriorityFiber()
        {
            uint ans = 0;


            var temp = MyMath.ListOfLcm(priorities);
            int a = random.Next(temp[temp.Count - 1]);
            int i = 0;
            while (i < temp.Count && temp[i] <= a)
                i++;
            var currentPriorities = GetFibersWithDefinitePriority(priorities[Math.Min(i, priorities.Count - 1)] - 1); // compensation 39 line
            ans = currentPriorities[random.Next(currentPriorities.Count)];

            return ans;
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
            priorities = null;
            random = null;
        }

        // for tests
        public static List<uint> GetDelList()
        {
            return delList;
        }
    }
}
