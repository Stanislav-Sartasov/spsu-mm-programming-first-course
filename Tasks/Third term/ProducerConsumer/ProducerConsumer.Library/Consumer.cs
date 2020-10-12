using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProducerConsumer.Library
{
    public class Consumer<T> where T :class
    {
        private string name;
        private Thread thread;
        public bool continueRun { get; private set; }

        public Consumer(string name)
        {
            this.name = name;
            continueRun = true;
            thread = new Thread(Run);
        }

        private void Run(object obj)
        {
            while (continueRun)
            {
                Manager<T>.Take(name);
                Thread.Sleep(1000);
            }
        }

        internal void Start()
        {
            thread.Start();
        }

        internal void Exit()
        {
            continueRun = false;
            thread.Join();
            Console.WriteLine($"Consumer {name} exit");
        }
    }
}
