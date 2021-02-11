using System;

namespace ExamSystem
{
    public class ExamInfo : IEquatable<ExamInfo>, IComparable<ExamInfo>
    {
        public long StudentId { get; }
        public long CourseId { get; }

        public ExamInfo(long studentId, long courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
        }

        public override int GetHashCode() => 
            HashCode.Combine(StudentId, CourseId);

        public override bool Equals(object obj) =>
           obj is null ? false : obj is ExamInfo && Equals((ExamInfo)obj);

        public bool Equals(ExamInfo other) => 
            StudentId.Equals(other.StudentId) && CourseId.Equals(other.CourseId);

        public int CompareTo(ExamInfo other)
        {
            if (other is null) 
                return 1;

            var comp = StudentId.CompareTo(other.StudentId);
            return comp == 0 ? CourseId.CompareTo(other.CourseId) : comp;
        }
    }
}
