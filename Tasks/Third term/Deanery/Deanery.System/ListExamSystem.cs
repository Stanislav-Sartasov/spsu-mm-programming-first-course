using System;
using System.Collections.Generic;
using System.Text;
using SpinLockList;

namespace Deanery.System
{
    public class ListExamSystem : IExamSystem
    {
        private volatile MySpinLockList<(long, long)>[] table;
        private const int size = 9999;
        public ListExamSystem()
        {
            table = new MySpinLockList<(long, long)>[size + 1];
            for (int i = 1; i < size + 1; i++)
                table[i] = new MySpinLockList<(long, long)>();
        }
        public int GetSizeOfHashTable()
        {
            return size;
        }

        private long GetHash(long id)
        {
            return id % size + 1;
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
