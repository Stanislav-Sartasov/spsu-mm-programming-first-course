using System;
using System.Collections.Generic;
using System.Text;

namespace DeansOffice.ExamSystems
{
    public class MutexListDeanery : IExamSystem
    {
        private volatile MutexList<(long, long)>[] table;
        private readonly int size;

        public MutexListDeanery(int size)
        {
            this.size = size;
            table = new MutexList<(long, long)>[size];
            for (int i = 0; i < size; i++)
                table[i] = new MutexList<(long, long)>();
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
            return table[GetHash(studentId)].Find((studentId, courseId)) != -1;
        }

        public void Remove(long studentId, long courseId)
        {
            table[GetHash(studentId)].Remove((studentId, courseId));
        }
    }
}