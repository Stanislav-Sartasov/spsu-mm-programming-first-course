using System;
using System.Collections.Generic;
using System.Text;

namespace DeansOffice.ExamSystems
{
    public class TTASDeanery : IExamSystem
    {
        private volatile List<(long, long)>[] hashTable;
        private readonly int size;
        public TTASDeanery(int size)
        {
            this.size = size;
            hashTable = new List<(long, long)>[size];
            for (int i = 0; i < size; i++)
                hashTable[i] = new List<(long, long)>();
        }
        public int GetSizeOfHashTable()
        {
            return size;
        }
        public List<(long, long)>[] GetHashTable()
        {
            return hashTable;
        }

        private long GetHash(long id)
        {
            return id % size;
        }
        public void Add(long studentId, long courseId)
        {
            TTASLock.Lock();
            hashTable[GetHash(studentId)].Add((studentId, courseId));
            TTASLock.Unlock();
        }

        public bool Contains(long studentId, long courseId)
        {
            return hashTable[GetHash(studentId)].Contains((studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            TTASLock.Lock();
            hashTable[GetHash(studentId)].Remove((studentId, courseId));
            TTASLock.Unlock();
        }
    }
}
