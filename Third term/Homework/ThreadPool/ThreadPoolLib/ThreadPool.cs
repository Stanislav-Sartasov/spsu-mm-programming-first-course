using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace ThreadPoolLib
{
    public class ThreadPool : IDisposable
    {
        private readonly object poolLock;

        volatile bool isWorking;

        private bool isDisposed;

        private const int NumberOfThreads = 8;

        private Queue<Action> taskQueue;

        private List<TaskHandler> pool;


        public void Enqueue(Action a)
        {
            if (isWorking)
            {
                lock (poolLock)
                {
                    taskQueue.Enqueue(a);
                    Monitor.PulseAll(poolLock);
                }
            }
            else
            {
                throw new InvalidOperationException("Thread pool has been disposed");
            }
        }

        public ThreadPool()
        {
            isWorking = true;
            isDisposed = false;
            poolLock = new object();
            taskQueue = new Queue<Action>();
            pool = new List<TaskHandler>();

            for (int i = 0; i < NumberOfThreads; i++)
            {
                TaskHandler th = new TaskHandler(null, this);
                pool.Add(th);
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
            
            lock (poolLock)
            {
                isWorking = false;
                Monitor.PulseAll(poolLock);
            }
            foreach (var th in pool)
            {
                th.Join();
            }

            if (disposing)
            {
                taskQueue.Clear();
                pool.Clear();
            }

            isDisposed = true;
        }

        private class TaskHandler
        {
            private Thread handler;
            private ThreadPool pool;
            private Action task;

            public TaskHandler(Action task, ThreadPool pool)
            {
                this.pool = pool;
                this.task = task;
                handler = new Thread(DoWork)
                {
                    IsBackground = true
                };
                handler.Start();
            }

            public void Join() => handler.Join();

            private void DoWork()
            {
                while (pool.isWorking)
                {
                    task = null;
                    lock (pool.poolLock)
                    {
                        if (pool.taskQueue.Any())
                        {
                            task = pool.taskQueue.Dequeue();
                        }
                        else
                        {
                            Monitor.Wait(pool.poolLock);
                        }
                    }
                    task?.Invoke();
                }
            }
        }
    }
}
