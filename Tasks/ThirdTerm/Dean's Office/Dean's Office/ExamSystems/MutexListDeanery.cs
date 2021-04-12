using System;
using System.Collections.Generic;
using System.Text;

namespace DeansOffice.ExamSystems
{
    public class MutexListDeanery : IExamSystem
    {
        private volatile List<(long, long)> list;
        public MutexListDeanery()
        {
            list = new List<(long, long)>();
        }

        public List<(long, long)> GetList()
        {
            return list;
        }

        public void Add(long studentId, long courseId)
        {
            list.Add((studentId, courseId));
        }

        public bool Contains(long studentId, long courseId)
        {
            return list.Contains((studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            list.Remove((studentId, courseId));
        }
    }
}
