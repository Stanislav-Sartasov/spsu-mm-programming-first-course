using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Task9WF
{
    public class TaskManager
    {
        public TaskManager(out StopDelegate stop)
        {
            taskList.Add(Task.Run(() => Ref()));
            stop = Stop;
        }

        public delegate void StopDelegate();
        List<Task> taskList = new List<Task>();
        bool stopped = false;
        object locker = new object();
        public void Add(Task task)
        {
            lock (locker)
            {
                taskList.Add(task);
            }
        }
        public Task Run(Action action)
        {
            Task task = Task.Run(action);
            lock (locker)
            {
                taskList.Add(task);
            }
            return task;
        }
        void Ref()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (stopped)
                    return;
                lock (locker)
                {
                    for (int i = 0; i < taskList.Count;)
                        if (taskList[i].IsCompleted)
                            taskList.RemoveAt(i);
                        else
                            i++;
                }
            }
        }
        void Stop()
        {
            stopped = true;
            lock (locker)
            {
                Task.WaitAll(taskList.ToArray());
            }
        }
    }
}
