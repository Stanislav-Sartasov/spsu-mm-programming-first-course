using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
    public class ThreadPool : IDisposable
    {
        private object taskLock;

        private object poolLock;

        private bool isDisposed;

        private const int NumberOfThreads = 10;

        private Queue<Action> taskQueue;

        private List<TaskHandler> pool;

        private Thread taskScheduler;

        public void Enqueue(Action a)
        {
            lock (taskLock)
            {
                taskQueue.Enqueue(a);
            }
        }

        public ThreadPool()
        {
            isDisposed = false;

            taskLock = new object();
            poolLock = new object();
            taskQueue = new Queue<Action>();
            pool = new List<TaskHandler>();

            for (int i = 0; i < NumberOfThreads; i++)
            {
                TaskHandler task = new TaskHandler(() => { });
                AddTask(task);
            }

            taskScheduler = new Thread(() =>
            {
                while (!isDisposed)
                {
                    lock (taskLock)
                    {
                        int tasksCount = taskQueue.Count;
                        for (int i = 0; i < tasksCount; i++)
                        {
                            var nextTask = taskQueue.Peek();
                            lock (poolLock)
                            {
                                foreach (var t in pool)
                                {
                                    lock (t)
                                    {
                                        if (t.Status != TaskStatus.NotStarted)
                                        {
                                            t.Task = nextTask;
                                            t.Status = TaskStatus.NotStarted;
                                            taskQueue.Dequeue();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Thread.Yield();
                }
            })
            {
                Priority = ThreadPriority.AboveNormal
            };
            taskScheduler.Start();
        }

        private void AddTask(TaskHandler task)
        {
            lock (poolLock)
            {
                pool.Add(task);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ThreadPool() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;
            while (taskQueue.Count != 0) ;

            isDisposed = true;

            if (disposing)
            {
                lock (poolLock)
                {
                    taskScheduler.Join();
                    foreach (var t in pool)
                        t.Dispose();
                    pool.Clear();
                }
                lock (taskLock)
                {
                    taskQueue.Clear();
                }
            }
        }
    }
}
