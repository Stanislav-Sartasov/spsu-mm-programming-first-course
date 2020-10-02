using Fibers.Processes;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Fibers
{
    class Program
    {
        static IProcess digitalRoot = Creator.Create(2);
        static IProcess fibonacci = Creator.Create(1);
        static List<Tuple<Thread, ManualResetEvent>> threads = new List<Tuple<Thread, ManualResetEvent>>()
                                                                    { Tuple.Create(new Thread(new ThreadStart(fibonacci.Process)), new ManualResetEvent(false)),
                                                                      Tuple.Create(new Thread(new ThreadStart(digitalRoot.Process)), new ManualResetEvent(false)) };
        static void Main(string[] args)
        {
            List<IProcess> processes = new List<IProcess>() { fibonacci, digitalRoot };


            //threads[0].Item1.Name = "Fibonacci";
            //threads[1].Item1.Name = "Digital root";
            //foreach (var thread in threads)
            //{
            //    thread.Item1.Start();
            //    //thread.Item2.WaitOne();
            //   // Thread.Sleep(100);
            //    thread.Item2.Set();
            //}
            bool processing = true;
            int i = 0;
            while (processing)
            {
                threads[i].Item2.Set();
                Thread.Sleep(100);
                threads[i].Item2.Reset();
                //processing = false;
                if (!threads[i].Item1.IsAlive)
                {
                    threads.RemoveAt(i);
                }
                i = (i + 1) % threads.Count;
            }
        }
    }
}
