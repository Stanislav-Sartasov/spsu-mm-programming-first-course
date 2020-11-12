using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProdusersConsumers
{
    public class Consumer : IDisposable
    {
        private Thread thread;
        private volatile bool isWorking;
        private Manager manager;
        public Consumer(Manager manager)
        {
            this.manager = manager;
            isWorking = true;
            thread = new Thread(new ParameterizedThreadStart(Run));
        }

        public void Run(object obj)
        {
            while (isWorking)
            {
                manager.GetProduct(thread);
                Thread.Sleep(1000);
            }
        }
        public void Start()
        {
            thread.Start();
        }
        public void Dispose()
        {
            isWorking = false;
            thread.Join();
        }
    }
}
