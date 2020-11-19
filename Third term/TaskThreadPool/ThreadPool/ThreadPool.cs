using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
    public class ThreadPool : IDisposable
    {
        private volatile bool isWorking;
        private bool disposed = false;
        private const int numberOfThreads = 8;
        private Queue<Action> taskQueue;
        private LinkedList<Thread> threadList;
        public ThreadPool()
        {
            isWorking = true;
            threadList = new LinkedList<Thread>();
            taskQueue = new Queue<Action>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                Thread thread = new Thread(Work);
                thread.Name = i.ToString();
                thread.IsBackground = true;
                threadList.AddLast(thread);
                threadList.Last.Value.Start();
            }
        }

        private void Work()
        {
            Action task;
            while (isWorking)
            {
                Monitor.Enter(taskQueue);
                if (taskQueue.Count > 0)
                {
                    Console.WriteLine("Thread {0} took task.", Thread.CurrentThread.Name);
                    task = taskQueue.Dequeue();
                    Monitor.PulseAll(taskQueue);
                    Monitor.Exit(taskQueue);
                    task?.Invoke();
                }
                else
                    Monitor.Exit(taskQueue);
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

        ~ThreadPool() => Dispose(false);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (disposed)
                return;

            lock (taskQueue)
            {
                isWorking = false;
                Monitor.PulseAll(taskQueue);
            }

            foreach (Thread thread in threadList)
            {
                thread.Join();
            }
                

            if (disposing)
            {
                taskQueue.Clear();
                threadList.Clear();
            }
            disposed = true;

        }

        public int GetTasksCount()
        {
            return taskQueue.Count;
        }
        public int GetThreadsCount()
        {
            return threadList.Count;
        }
    }
}
