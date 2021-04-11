using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HTLib
{
	public class RefinableHT : IExamSystem
	{
		private List<Node>[] table;
		private int sizeOfTables;
		private int currElements;

		private volatile ResizeLock resizeLock; //emulating some locks of AtomicMarkableReference with mutex in separate class
		private volatile Mutex[] locks;

		public RefinableHT(int capacity)
		{
			sizeOfTables = capacity;
			currElements = 0;

			table = new List<Node>[sizeOfTables];
			locks = new Mutex[sizeOfTables];

			for (int i = 0; i < sizeOfTables; i++)
			{
				table[i] = new List<Node>();
				locks[i] = new Mutex();
			}

			resizeLock = new ResizeLock();
		}

		private bool NeedResize
		{
			get
			{
				return currElements / sizeOfTables > 4;
			}
		}

		private long GetHash(long id, int div)
		{
			return id.GetHashCode() % div;
		}

		private void Acquire(long id)
		{
			var currThread = Thread.CurrentThread;
			while (true)
			{
				do { } while (resizeLock.ResizeNotAvaiable(currThread));

				var oldLocks = locks;
				var oldLock = oldLocks[GetHash(id, sizeOfTables)];
				oldLock.WaitOne();

				if ((!resizeLock.ResizeNotAvaiable(currThread)) && (locks == oldLocks))
				{
					return;
				}
				else
				{
					oldLock.ReleaseMutex();
				}
			}
		}
		private void Release(long id)
		{
			locks[GetHash(id, sizeOfTables)].ReleaseMutex();
		}

		private void Resize()
		{
			resizeLock.Lock();
			int oldCapacity = sizeOfTables;

			if (resizeLock.IsEmpty)
			{
				resizeLock.SetOwner(Thread.CurrentThread);
				resizeLock.Unlock();

				try
				{
					if (sizeOfTables != oldCapacity)
					{
						return;
					}

					foreach (var mutex in locks)
					{
						mutex.WaitOne();
						mutex.ReleaseMutex();
					}

					sizeOfTables *= 2;

					var oldTable = table;
					table = new List<Node>[sizeOfTables];
					locks = new Mutex[sizeOfTables];

					for (int i = 0; i < sizeOfTables; i++)
					{
						table[i] = new List<Node>();
						locks[i] = new Mutex();
					}
					foreach (var bucket in oldTable)
					{
						foreach (var node in bucket)
						{
							Add(node.StudentID, node.CourseID);
						}
					}
				}
				finally
				{
					resizeLock.Reset();
				}
			}
			else
			{
				resizeLock.Unlock();
			}
		}

		public void Add(long studentID, long courseID)
		{
			Acquire(studentID);
			
			try
			{
				long hash = GetHash(studentID, sizeOfTables);

				if (!table[hash].Any(x => x.StudentID == studentID && x.CourseID == courseID))
				{
					table[hash].Add(new Node(studentID, courseID));
					Interlocked.Increment(ref currElements);
				}
			}
			finally
			{
				Release(studentID);
			}

			if (NeedResize)
			{
				Resize();
			}
		}

		public void Remove(long studentID, long courseID)
		{
			Acquire(studentID);

			try
			{
				long hash = GetHash(studentID, sizeOfTables);

				if (table[hash].Any(x => x.StudentID == studentID && x.CourseID == courseID))
				{
					table[hash].Remove(table[hash].Find(x => x.StudentID == studentID && x.CourseID == courseID));
					Interlocked.Decrement(ref currElements);
				}
			}
			finally
			{
				Release(studentID);
			}
		}

		public bool Contains(long studentID, long courseID)
		{
			Acquire(studentID);

			try
			{
				long hash = GetHash(studentID, sizeOfTables);

				return table[hash].Any(x => x.StudentID == studentID && x.CourseID == courseID);
			}
			finally
			{
				Release(studentID);
			}
		}
	}
}