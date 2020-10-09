using Fibers.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                ProcessManager.AddProcess(new Framework.Process());
            }
            ProcessManager.Switch(false);
            Thread.Sleep(10);
            //ProcessManager.Dispose();
        }
    }
}
