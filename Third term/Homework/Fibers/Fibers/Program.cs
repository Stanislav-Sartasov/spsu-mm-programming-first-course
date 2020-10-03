using FibersLib;
using System.Threading;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessManager.IsPriority = true;
            for (int i = 0; i < 10; i++)
            {
                ProcessManager.AddProcess(new Process());
            }
            ProcessManager.Run();
            Thread.Sleep(10);
            ProcessManager.Dispose();
        }
    }
}
