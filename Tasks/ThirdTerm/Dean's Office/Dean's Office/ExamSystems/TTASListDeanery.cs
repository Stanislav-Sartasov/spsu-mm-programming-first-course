using System;
using System.Collections.Generic;
using System.Text;

namespace DeansOffice.ExamSystems
{
    public class TTASListDeanery : IExamSystem
    {
        private volatile TTASList<(long, long)>[] hashTable;
        private readonly int size;
        public TTASListDeanery(int size)
        {
            this.size = size;
            hashTable = new TTASList<(long, long)>[size];
            for (int i = 0; i < size; i++)
                hashTable[i] = new TTASList<(long, long)>((i - 1, i - 1));
        }
        public int GetSizeOfHashTable()
        {
            return size;
        }
        public TTASList<(long, long)>[] GetHashTable()
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
            TTASLock.Lock();
            bool answer = hashTable[GetHash(studentId)].Contains((studentId, courseId));
            TTASLock.Unlock();
            return answer;
        }

        public void Remove(long studentId, long courseId)
        {
            TTASLock.Lock();
            hashTable[GetHash(studentId)].Remove((studentId, courseId));
            TTASLock.Unlock();
        }
    }
}
