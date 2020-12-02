using System;
using System.Collections.Generic;
using System.Text;
using SpinLockList;

namespace Deanery.System
{
    public class ListExamSystem : IExamSystem, IHashTable
    {
        private volatile MySpinLockList<(long, long)>[] table;
        private readonly int size;
        public ListExamSystem(int size)
        {
            this.size = size;
            table = new MySpinLockList<(long, long)>[size + 1];
            for (int i = 1; i < size + 1; i++)
                table[i] = new MySpinLockList<(long, long)>((i - 1, i - 1));
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
            return table[GetHash(studentId)].Contains((studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            table[GetHash(studentId)].Remove((studentId, courseId));
        }

        public object GetTable()
        {
            return table;
        }
    }
}
