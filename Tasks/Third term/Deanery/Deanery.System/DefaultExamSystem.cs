using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Deanery.System
{
    public class DefaultExamSystem : IExamSystem
    {
        private volatile List<(long, long)>[] table;
        private const int size = 9999;
        public DefaultExamSystem()
        {
            table = new List<(long, long)>[size];
            for (int i = 0; i < size; i++)
                table[i] = new List<(long, long)>();
        }
        public int GetSizeOfHashTable()
        {
            return size;
        }

        private long GetHash(long id)
        {
            return id % size;
        }
        public void Add(long studentId, long courseId)
        {
            Monitor.Enter(table);

            table[GetHash(studentId)].Add((studentId, courseId));

            Monitor.Exit(table);
        }

        public bool Contains(long studentId, long courseId)
        {
            return table[GetHash(studentId)].Contains((studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            Monitor.Enter(table);

            table[GetHash(studentId)].Remove((studentId, courseId));

            Monitor.Exit(table);
        }
    }
}
