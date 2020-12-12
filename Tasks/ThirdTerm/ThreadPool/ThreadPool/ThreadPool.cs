using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadPool
{
    public class ThreadPool : IDisposable
    {
        private bool disposedValue;
        private volatile bool isWorking;
        private bool disposed = false;
        private const int numberOfThreads = 8;
        private Queue<Action> taskQueue;
        private List<Thread> threads;

        public ThreadPool()
        {
            isWorking = true;
            threads = new List<Thread>();
            taskQueue = new Queue<Action>();

            for (int i = 0; i < numberOfThreads; i++)
            {
                Thread thread = new Thread(Work)
                {
                    Name = i.ToString(),
                    IsBackground = true
                };
                threads.Add(thread);
                threads[i].Start();
            }
        }

        public void Enqueue(Action a)
        {
            if (isWorking)
            {
                lock (taskQueue)
                {
                    taskQueue.Enqueue(a);
                    Monitor.PulseAll(taskQueue);
                }
            }
        }

        public void Work()
        {
            Action task;

            while (isWorking)
            {
                Monitor.Enter(taskQueue);
                if (taskQueue.Count > 0)
                {
                    Console.WriteLine($"Thread {Thread.CurrentThread.Name} took task.");
                    task = taskQueue.Dequeue();
                    Monitor.PulseAll(taskQueue);
                    Monitor.Exit(taskQueue);
                    task?.Invoke();
                }
                else
                {
                    Monitor.Wait(taskQueue);
                    Monitor.Exit(taskQueue);
                }
            }
        }

        public int GetTasksCount()
        {
            return taskQueue.Count;
        }

        public int GetThreadsCount()
        {
            return threads.Count;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                lock (taskQueue)
                {
                    isWorking = false;
                    Monitor.PulseAll(taskQueue);
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

                if (disposing)
                {
                    taskQueue.Clear();
                    threads.Clear();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~ThreadPool()
        {
            Dispose(disposing: false);
        }
    }
}
