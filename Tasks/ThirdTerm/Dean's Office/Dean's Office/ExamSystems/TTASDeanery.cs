using System;
using System.Collections.Generic;
using System.Text;

namespace DeansOffice.ExamSystems
{
    public class TTASDeanery : IExamSystem
    {
        private volatile List<(long, long)> list;
        public TTASDeanery()
        {
            list = new List<(long, long)>();
        }
        public int GetSizeOfList()
        {
            return list.Count;
        }
        public List<(long, long)> GetList()
        {
            return list;
        }

        public void Add(long studentId, long courseId)
        {
            TTASLock.Lock();
            list.Add((studentId, courseId));
            TTASLock.Unlock();
        }

        public bool Contains(long studentId, long courseId)
        {
            return list.Contains((studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            TTASLock.Lock();
            list.Remove((studentId, courseId));
            TTASLock.Unlock();
        }
    }
}
