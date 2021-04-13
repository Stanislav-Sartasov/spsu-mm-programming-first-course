using System;
using System.Collections.Generic;
using System.Text;

namespace Task6.LazySet
{
	public class LazySet : IExamSystem
	{
		volatile Elem<(long, long)> head = new Elem<(long, long)>() { Next = new Elem<(long, long)>() };

		static bool Validate(Elem<(long, long)> pred, Elem<(long, long)> curr)
		{
			return !pred.Marked && !curr.Marked && pred.Next == curr;
		}
		public void Add(long studentId, long courseId)
		{
			while (true)
			{
				bool valid = true;
				Elem<(long, long)> pred = head;
				Elem<(long, long)> curr = pred.Next;

				int key = (studentId, courseId).GetHashCode();

				while (curr.Key <= key)
				{
					if (curr.Next == null)
					{
						pred.Locker.Lock();
						curr.Locker.Lock();
						if (!Validate(pred, curr))
						{
							valid = false;
							pred.Locker.Unlock();
							curr.Locker.Unlock();
							break;
						}
						
						curr.Key = key;
						curr.Value = (studentId, courseId);
						curr.Next = new Elem<(long, long)>();
						pred.Locker.Unlock();
						curr.Locker.Unlock();
						return;
					}
					else if (curr.Key == key)
					{
						pred.Locker.Lock();
						curr.Locker.Lock();

						if (curr.Value != (studentId, courseId))
						{
							pred.Locker.Unlock();
							curr.Locker.Unlock();
							continue;
						}

						if (!Validate(pred, curr))
						{
							valid = false;
							pred.Locker.Unlock();
							curr.Locker.Unlock();
							break;
						}

						pred.Locker.Unlock();
						curr.Locker.Unlock();
						return;
					}
					else
					{
						pred = curr;
						curr = curr.Next;
					}
				}

				if (!valid)
					continue;

				pred.Locker.Lock();
				curr.Locker.Lock();
				if (!Validate(pred, curr))
				{
					pred.Locker.Unlock();
					curr.Locker.Unlock();
					break;
				}

				pred.Next = new Elem<(long, long)>()
				{
					Key = key,
					Value = (studentId, courseId),
					Next = curr
				};

				pred.Locker.Unlock();
				curr.Locker.Unlock();
				return;
			}
		}
		public void Remove(long studentId, long courseId)
		{
			while (true)
			{
				bool valid = true;
				Elem<(long, long)> pred = head;
				Elem<(long, long)> curr = pred.Next;

				int key = (studentId, courseId).GetHashCode();

				while (curr.Key <= key)
				{
					if (curr.Next == null)
						return;
					else if (curr.Key == key)
					{
						pred.Locker.Lock();
						curr.Locker.Lock();

						if (curr.Value != (studentId, courseId))
						{
							pred.Locker.Unlock();
							curr.Locker.Unlock();
							continue;
						}

						if (!Validate(pred, curr))
						{
							valid = false;
							pred.Locker.Unlock();
							curr.Locker.Unlock();
							break;
						}

						curr.Marked = true;
						pred.Next = curr.Next;

						pred.Locker.Unlock();
						curr.Locker.Unlock();
						return;
					}
					else
					{
						pred = curr;
						curr = curr.Next;
					}
				}

				if (!valid)
					continue;

				return;
			}
		}

		public bool Contains(long studentId, long courseId)
		{
			Elem<(long, long)> curr = head.Next;

			int key = (studentId, courseId).GetHashCode();

			while (curr.Key <= key)
			{
				if (curr.Next == null)
					return false;
				else if (curr.Key == key)
				{
					curr.Locker.Lock();

					if (curr.Value != (studentId, courseId))
					{
						curr.Locker.Unlock();
						continue;
					}

					bool res = !curr.Marked;
					curr.Locker.Unlock();

					return res;
				}
				else
					curr = curr.Next;
			}

			return false;
		}
	}
}
