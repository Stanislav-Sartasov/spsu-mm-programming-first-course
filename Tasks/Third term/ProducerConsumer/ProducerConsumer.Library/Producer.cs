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
        private Manager<T> myManager;
        private volatile bool continueRun;

        public Producer(string name, Manager<T> manager)
        {
            myManager = manager;
            this.name = name;
            continueRun = true;
            thread = new Thread(Run);
        }

        private void Run(object obj)
        {
            while (continueRun)
            {
                myManager.Put(name, default(T));
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
            Console.WriteLine($"Producer {name} exit");
        }
        public bool IsRunning()
        {
            return continueRun;
        }
    }
}
