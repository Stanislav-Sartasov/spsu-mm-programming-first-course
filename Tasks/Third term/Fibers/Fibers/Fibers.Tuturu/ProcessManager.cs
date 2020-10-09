using System;
using System.Collections.Generic;
using System.Linq;

namespace Fibers.Tuturu
{
    public static class ProcessManager
    {
        private static uint currentFiber;
        private static Dictionary<uint, Process> tasks = new Dictionary<uint, Process>();
        private static Random random = new Random();

        public static void AddProcess(Process process)
        {
            Fiber fiber = new Fiber(process.Run);
            tasks[fiber.Id] = process;
        }
        public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                Console.WriteLine($"Fiber {currentFiber} finished");
                tasks.Remove(currentFiber);
            }
            currentFiber = GetNextFiber();
            Fiber.Switch(currentFiber);
        }

        private static uint GetNextFiber()
        {
            if (tasks.Count == 0)
                return Fiber.PrimaryId;
            var keys = tasks.Keys;
            uint id = keys.ElementAt<uint>(random.Next(keys.Count));
            return id;
        }
    }
}
