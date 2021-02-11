using ExamSystem.ConcurrentCollections;
using ExamSystem.Interfaces;
using System.Collections.Generic;

namespace ExamSystem
{
    public class StripedExamSystem : IExamSystem
    {
        StripedHashSet<ExamInfo> examInfos;

        public StripedExamSystem(int capicity, ILockFactory lockFactory) =>
            examInfos = new StripedHashSet<ExamInfo>(capicity, lockFactory);

        public int Size() => examInfos.Size;

        public List<ExamInfo> GetAllData() => examInfos.GetAllData();

        public void Add(long studentId, long courseId) => 
            examInfos.Add(new ExamInfo(studentId, courseId));

        public bool Contains(long studentId, long courseId) => 
            examInfos.Contains(new ExamInfo(studentId, courseId));

        public void Remove(long studentId, long courseId) => 
            examInfos.Remove(new ExamInfo(studentId, courseId));
    }
}
