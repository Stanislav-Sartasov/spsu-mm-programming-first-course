using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Task6.StaticSizeHashSet
{
	public class StaticSizeHashSet : IExamSystem
	{
		public StaticSizeHashSet(int size)
		{
			table = new Bin[size];
			for (int i = 0; i < size; i++)
				table[i] = new Bin();
		}

		volatile Bin[] table;

		public void Add(long studentId, long courseId)
		{
			table[studentId.GetHashCode() % table.Length].Add(studentId, courseId);
		}
		public void Remove(long studentId, long courseId)
		{
			table[studentId.GetHashCode() % table.Length].Remove(studentId, courseId);
		}
		public bool Contains(long studentId, long courseId)
		{
			return table[studentId.GetHashCode() % table.Length].Contains(studentId, courseId);
		}
	}
}
