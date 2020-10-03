using System;
using System.Collections.Generic;
using System.Threading;


namespace ThreadPoolLib
{
    public class ThreadPool : IDisposable
    {
        private readonly object taskLock;

        private readonly object poolLock;

        private volatile bool isDisposed;

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
                TaskHandler th = new TaskHandler(() => { });
                AddTaskHandler(th);
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
                            SetNextTask(nextTask);
                            taskQueue.Dequeue();
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

        private void SetNextTask(Action nextTask)
        {
            lock (poolLock)
            {
                foreach (var th in pool)
                {
                    lock (th)
                    {
                        if (th.Status != TaskStatus.NotStarted)
                        {
                            th.Task = nextTask;
                            th.Status = TaskStatus.NotStarted;
                            break;
                        }
                    }
                }
            }
        }

        private void AddTaskHandler(TaskHandler task)
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
                    foreach (var th in pool)
                        th.Dispose();
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
