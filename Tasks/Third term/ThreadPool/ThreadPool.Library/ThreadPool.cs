using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool.Library
{
    public class ThreadPool : IDisposable
    {
        private const int numberOfThreads = 5;
        private Queue<Action> tasks;
        private List<Thread> threads;
        private bool continueCalc;

        public void Start()
        {
            Initialize();
            for (int i = 0; i < numberOfThreads; i++)
                threads[i].Start();
        }

        private void Initialize()
        {
            tasks = new Queue<Action>();
            threads = new List<Thread>(numberOfThreads);
            continueCalc = true;
            for (int i = 0; i < numberOfThreads; i++)
            {
                Thread thread = new Thread(Run);
                thread.Name = $"My thread {i}";
                threads.Add(thread);
            }
        }

        public void Enqueue(Action action)
        {
            Monitor.Enter(tasks);

            tasks.Enqueue(action);

            Monitor.PulseAll(tasks);
            Monitor.Exit(tasks);
        }

        private void Run()
        {
            while(continueCalc)
            {
                Monitor.Enter(tasks);
                if (tasks.Count == 0)
                {
                    Monitor.Exit(tasks);
                    Thread.Sleep(100);
                    continue;
                }
                Action action = tasks.Dequeue();
                Console.WriteLine($"{Thread.CurrentThread.Name} works");
                action();
                Monitor.PulseAll(tasks);
                Monitor.Exit(tasks);
            }
        }
        public void Stop()
        {
            continueCalc = false;
            for (int i = 0; i < numberOfThreads; i++)
                if (threads[i].IsAlive)
                    threads[i].Join();
        }

        public void Continue()
        {
            if (!continueCalc)
            {
                continueCalc = true;
                for (int i = 0; i < numberOfThreads; i++)
                {
                    threads[i] = new Thread(Run);
                    threads[i].Start();
                }
            }
        }

        public Queue<Action> GetTasks()
        {
            return tasks;
        }
        public List<Thread> GetThreads()
        {
            return threads;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isUser)
        {
            if (!continueCalc)
                return;
            Stop();
            if (isUser)
            {
                threads.Clear();
                tasks.Clear();
            }
        }

        ~ThreadPool() => Dispose(false);
    }
}
