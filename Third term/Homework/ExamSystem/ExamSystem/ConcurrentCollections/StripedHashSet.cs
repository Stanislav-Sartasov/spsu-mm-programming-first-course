using ExamSystem.Interfaces;
using System.Threading;
using System.Linq;
using System;
using System.Collections.Generic;


namespace ExamSystem.ConcurrentCollections
{
    public class StripedHashSet<T>
    {
        ILock[] locks;
        List<T>[] buckets;
        int size;
        public int Size { get => size; }

        //only for test
        public List<T> GetAllData() => 
            buckets.SelectMany(item => item).ToList();

        public StripedHashSet(int capacity, ILockFactory lockFactory)
        {
            if (capacity <= 1)
                throw new ArgumentException("Capacity must be greater than 1");
            size = 0;
            buckets = new List<T>[capacity];
            locks = new ILock[capacity];
            for (int i = 0; i < capacity; i++)
            {
                buckets[i] = new List<T>();
                locks[i] = lockFactory.CreateLock();
            }
        }

        private int GetIndex(T item, int size) => 
            Math.Abs(item.GetHashCode() % size);

        private void Acquire(T item) => 
            locks[GetIndex(item, locks.Length)].Lock();

        private void Release(T item) => 
            locks[GetIndex(item, locks.Length)].Unlock();

        public bool Contains(T item)
        {
            Acquire(item);
            try
            {
                return buckets[GetIndex(item, buckets.Length)].Contains(item);
            }
            finally
            {
                Release(item);
            }
        }

        public bool Add(T item)
        {
            var result = false;
            Acquire(item);
            try
            {
                var myBucket = GetIndex(item, buckets.Length);
                if (!buckets[myBucket].Contains(item))
                {
                    buckets[myBucket].Add(item);
                    result = true;
                    Interlocked.Increment(ref size);
                }
            }
            finally
            {
                Release(item);
            }
            if (size / buckets.Length > 4)
                Resize();
            return result;
        }

        public bool Remove(T item)
        {
            bool result = false;
            Acquire(item);
            try
            {
                int myBucket = GetIndex(item, buckets.Length);
                if (buckets[myBucket].Contains(item))
                {
                    buckets[myBucket].Remove(item);
                    result = true;
                    Interlocked.Decrement(ref size);
                }
            }
            finally
            {
                Release(item);
            }
            return result;
        }

        private void Resize()
        {
            int oldCapacity = buckets.Length;

            foreach (var l in locks)
            {
                l.Lock();
            }

            try
            {
                if (oldCapacity != buckets.Length)
                    return;

                int newCapacity = 2 * oldCapacity;
                List<T>[] oldTable = buckets;
                buckets = new List<T>[newCapacity];

                for (int i = 0; i < newCapacity; i++)
                    buckets[i] = new List<T>();

                foreach (var bucket in oldTable)
                {
                    foreach (var item in bucket)
                    {
                        buckets[GetIndex(item, buckets.Length)].Add(item);
                    }
                }
            }
            finally
            {
                foreach (var l in locks)
                {
                    l.Unlock();
                }
            }
        }
    }
}
