using Fibers.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> processes = new List<Process>();
            for (int i = 0; i < 5; i++)
                processes.Add(new Process());

            ProcessManager.Initialize(processes, true);
            ProcessManager.Run();
            ProcessManager.Dispose();
        }
    }
}
