using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DekanatLibrary
{
    public class Node
    {
        public long StudentID { get; private set; }
        public long CourseID { get; private set; }

        public Node(long student, long course)
        {
            StudentID = student;
            CourseID = course;
        }
    }
}
