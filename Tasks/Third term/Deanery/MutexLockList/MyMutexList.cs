using System;
using System.Collections.Generic;
using System.Threading;

namespace MutexLockList
{
    public class MyMutexList<T>
    {
        private volatile List<T> data = null;
        private volatile Mutex mutex;

        public MyMutexList()
        {
            data = new List<T>();
            mutex = new Mutex();
        }

        public void Add(T value)
        {
            mutex.WaitOne();
            
            data.Add(value);

            mutex.ReleaseMutex();
        }

        public void Remove(T value)
        {
            mutex.WaitOne();

            data.Remove(value);

            mutex.ReleaseMutex();
        }

        public int Find(T value)
        {
            int res = data.IndexOf(value);

            return res;
        }
        public Mutex GetMutex()
        {
            return mutex;
        }
    }
}
