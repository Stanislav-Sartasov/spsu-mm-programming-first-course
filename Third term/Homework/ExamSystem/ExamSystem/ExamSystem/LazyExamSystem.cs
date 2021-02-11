using ExamSystem.Interfaces;
using System.Collections.Generic;
using ExamSystem.ConcurrentCollections;
using System.Linq;
using System;
using System.Threading;

namespace ExamSystem
{
    public class LazyExamSystem : IExamSystem
    {
        LazySet<ExamInfo>[] buckets;
        int size;
        public int Size { get => size; }

        public LazyExamSystem(int capacity, ILockFactory lockFactory)
        {
            if (capacity <= 1)
                throw new ArgumentException("Capacity must be greater than 1");
            size = 0;
            buckets = new LazySet<ExamInfo>[capacity];
            for (int i = 0; i < capacity; i++)
            {
                buckets[i] = new LazySet<ExamInfo>(lockFactory, 
                    new ExamInfo(long.MaxValue, long.MaxValue), 
                    new ExamInfo(long.MinValue, long.MinValue));
            }
        }

        private int GetIndex(ExamInfo item) =>
            Math.Abs(item.GetHashCode() % buckets.Length);

        public List<ExamInfo> GetAllData() => 
            buckets.SelectMany(b => b.GetAllData()).ToList();

        public void Add(long studentId, long courseId)
        {
            var item = new ExamInfo(studentId, courseId);
            if (buckets[GetIndex(item)].Add(item))
                Interlocked.Increment(ref size);
        }

        public void Remove(long studentId, long courseId)
        {
            var item = new ExamInfo(studentId, courseId);
            if (buckets[GetIndex(item)].Remove(item))
                Interlocked.Decrement(ref size);
        }

        public bool Contains(long studentId, long courseId)
        {
            var item = new ExamInfo(studentId, courseId);
            return buckets[GetIndex(item)].Сontains(item);
        }
    }
}
