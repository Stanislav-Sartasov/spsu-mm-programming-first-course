using System;
using System.Collections.Generic;
using System.Text;

namespace Task6
{
	public interface IExamSystem
	{
		public void Add(long studentId, long courseId);
		public void Remove(long studentId, long courseId);
		public bool Contains(long studentId, long courseId);
	}
}
