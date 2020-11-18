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
        private volatile bool continueCalc;
        private volatile int countThreads;

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
            countThreads = 0;
            for (int i = 0; i < numberOfThreads; i++)
            {
                countThreads++;
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
                while (tasks.Count == 0)
                {
                    Monitor.Wait(tasks);
                    if (!continueCalc)
                    {
                        Monitor.Exit(tasks);
                        return;
                    }
                }   
                Action action = tasks.Dequeue();
                Console.WriteLine($"{Thread.CurrentThread.Name} works");
                action();
                Monitor.Exit(tasks);
            }
        }
        public void Stop()
        {
            continueCalc = false;
            Monitor.Enter(tasks);
            Monitor.PulseAll(tasks);
            Monitor.Exit(tasks);
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
            Monitor.Enter(tasks);
            var temp = new Queue<Action>(tasks);
            Monitor.PulseAll(tasks);
            Monitor.Exit(tasks);
            return temp;
        }
        public int GetNumOfThreads()
        {
            return countThreads;
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
