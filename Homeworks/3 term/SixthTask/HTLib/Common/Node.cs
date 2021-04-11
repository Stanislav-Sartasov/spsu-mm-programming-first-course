namespace HTLib
{
	internal class Node
	{
		internal long StudentID { get; private set; } 
		internal long CourseID { get; private set; }

		internal Node(long studentID, long courseID)
		{
			StudentID = studentID;
			CourseID = courseID;
		}
	}
}