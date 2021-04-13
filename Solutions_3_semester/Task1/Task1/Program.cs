using System.Collections.Generic;
using Processes;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> processes = new List<Process>();
            for (int i = 0; i < 5; i++)
                processes.Add(new Process());

            ProcessManager.Init(processes.ToArray());
            ProcessManager.Priority = true;
            ProcessManager.Run();
            ProcessManager.Dispose();
        }
    }
}