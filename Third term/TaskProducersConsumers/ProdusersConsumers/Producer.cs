using System;
using System.Collections.Generic;
using System.Threading;

namespace ProdusersConsumers
{
    
    public class Producer : IDisposable
    {
        private Thread thread;
        private volatile bool isWorking;
        private Random random = new Random();
        private Manager manager;
        public Producer(Manager manager)
        {
            this.manager = manager;
            isWorking = true;
            thread = new Thread(new ParameterizedThreadStart(SetProduct));
        }
        public void Dispose()
        {
            isWorking = false;
            thread.Join();
        }
        public void SetProduct(object obj)
        {
            while (isWorking)
            {
                manager.SetProduct(thread, random.Next(100));
                Thread.Sleep(1000);
            }
        }
        public void Start()
        {
            thread.Start();
        }
    }
}
