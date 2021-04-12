using System;
using System.Collections.Generic;
using System.Threading;

namespace DeansOffice.ExamSystems
{
    public class MutexList<T>
    {
        private volatile List<T> list = null;
        private volatile Mutex mutex;

        public MutexList()
        {
            list = new List<T>();
            mutex = new System.Threading.Mutex();
        }

        public void Add(T value)
        {
            mutex.WaitOne();
            list.Add(value);
            mutex.ReleaseMutex();
        }

        public int Find(T value)
        {
            return list.IndexOf(value);
        }

        public void Remove(T value)
        {
            mutex.WaitOne();
            list.Remove(value);
            mutex.ReleaseMutex();
        }
    }
}
