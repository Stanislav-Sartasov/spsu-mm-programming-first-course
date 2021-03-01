using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiberLib;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {        
            for (int i = 0; i < 5; i++)
            {
                Process process = new Process();
                ProcessManager.AddProcess(process);
            }
            ProcessManager.Exec(SchedulerPriority.PriorityLevel);
            ProcessManager.Dispose();
            Console.WriteLine("Program ended");
            Console.ReadKey();
            
        }
    }
}
