using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProducerConsumer.Library
{
    public class Producer<T> where T : class
    {
        private string name;
        private Thread thread;
        public bool continueRun { get; private set; }

        public Producer(string name)
        {
            this.name = name;
            continueRun = true;
            thread = new Thread(Run);
        }

        private void Run(object obj)
        {
            while (continueRun)
                Manager<T>.Put(name, default(T));
        }

        internal void Start()
        {
            thread.Start();
        }

        internal void Exit()
        {
            continueRun = false;
            thread.Join();
            Console.WriteLine($"Producer {name} exit");
        }
    }
}
