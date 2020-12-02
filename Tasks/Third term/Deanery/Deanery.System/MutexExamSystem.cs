using MutexLockList;

using System;
using System.Collections.Generic;
using System.Text;

namespace Deanery.System
{
    public class MutexExamSystem : IExamSystem, IHashTable
    {
        private volatile MyMutexList<(long, long)>[] table;
        private readonly int size;
        public MutexExamSystem(int size)
        {
            this.size = size;
            table = new MyMutexList<(long, long)>[size];
            for (int i = 0; i < size; i++)
                table[i] = new MyMutexList<(long, long)>();
        }
        public int GetSizeOfHashTable()
        {
            return size;
        }
        public object GetTable()
        {
            return table;
        }

        private long GetHash(long id)
        {
            return id % size;
        }
        public void Add(long studentId, long courseId)
        {
            table[GetHash(studentId)].Add((studentId, courseId));
        }

        public bool Contains(long studentId, long courseId)
        {
            var a = table[GetHash(studentId)].Find((studentId, courseId));
            if (a != -1)
                return true;
            return false;
        }

        public void Remove(long studentId, long courseId)
        {
            table[GetHash(studentId)].Remove((studentId, courseId));
        }
    }
}
