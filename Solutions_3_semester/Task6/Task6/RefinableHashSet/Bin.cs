using System;
using System.Collections.Generic;
using System.Text;
using Task6;
using System.Collections;

namespace Task6.RefinableHashSet
{
	class Bin
	{
		public Locker Locker { get; } = new Locker();
		HashSet<(long, long)> content = new HashSet<(long, long)>();

		public void Add(long studentId, long courseId)
		{
			Locker.Lock();
			content.Add((studentId, courseId));
			Locker.Unlock();
		}
		public void Remove(long studentId, long courseId)
		{
			Locker.Lock();
			content.Remove((studentId, courseId));
			Locker.Unlock();
		}
		public bool Contains(long studentId, long courseId)
		{
			Locker.Lock();
			bool result = content.Contains((studentId, courseId));
			Locker.Unlock();
			return result;
		}
	}
}
