﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer.Library
{
    public class Consumer<T> : IDisposable where T : class
    {
        private string name;
        private Thread thread;
        private Manager<T> manager;
        private volatile bool isWorking;

        public Consumer(Manager<T> manager, string name)
        {
            this.name = name;
            this.manager = manager;
            isWorking = true;
            thread = new Thread(Run);
        }

        private void Run()
        {
            while (isWorking)
            {
                manager.Pop(name);
                Thread.Sleep(1000);
            }
        }

        public void Start()
        {
            thread.Start();
        }

        public bool IsWorking()
        {
            return isWorking;
        }

        public void Exit()
        {
            Dispose();
        }

        public void Dispose()
        {
            isWorking = false;
            thread.Join();
        }
    }
}
