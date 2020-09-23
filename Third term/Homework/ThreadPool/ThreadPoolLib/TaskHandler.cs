using System;
using System.Threading;

namespace ThreadPoolLib
{
    class TaskHandler : IDisposable
    {
        private object locker;
        private bool isWorking;
        private Thread handler;
        public Action Task { get; internal set; }
        
        public TaskStatus Status { get; internal set; }
        
        public TaskHandler(Action task)
        {
            locker = new object();
            isWorking = true;
            Task = task;
            Status = TaskStatus.Completed;
            handler = new Thread(DoWork);
            handler.Start();
        }

        private void DoWork()
        {
            while (isWorking)
            {
                if (Status != TaskStatus.Completed)
                {
                    lock (locker)
                    {
                        try
                        {
                            Task?.Invoke();
                            Status = TaskStatus.Completed;
                        }
                        catch
                        {
                            Status = TaskStatus.Interrupted;
                        }
                    }
                }
                Thread.Yield();
            }
        }

        public void Dispose()
        {
            isWorking = false;
            lock (locker)
            {
                Task = null;
            }
            handler.Join();
        }
    }
}
