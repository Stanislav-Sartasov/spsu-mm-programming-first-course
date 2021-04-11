using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HTLib
{
	public class StripedCuckooHT : IExamSystem
	{
		private const int threshold = 8;
		private const int sizeOfList = 16;
		private const int limit = 8;
		private int sizeOfTable;
		private int numOfMutexes;

		volatile private List<Node>[,] table;

		private Mutex[,] locks;

		public StripedCuckooHT(int capacity)
		{
			sizeOfTable = capacity;
			numOfMutexes = sizeOfTable;

			table = new List<Node>[2, sizeOfTable];
			locks = new Mutex[2, numOfMutexes];

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < sizeOfTable; j++)
				{
					table[i, j] = new List<Node>(sizeOfList);
					locks[i, j] = new Mutex();
				}
			}
		}

		private long GetFirstHash(long id, int div)
		{
			return (id.GetHashCode() + 2) % div;
		}
		private long GetSecondHash(long id, int div)
		{
			return (id.GetHashCode() + 1) % div;
		}

		private void Acquire(long id)
		{
			locks[0, GetFirstHash(id, numOfMutexes)].WaitOne();
			locks[1, GetSecondHash(id, numOfMutexes)].WaitOne();
		}
		private void Release(long id)
		{
			locks[0, GetFirstHash(id, numOfMutexes)].ReleaseMutex();
			locks[1, GetSecondHash(id, numOfMutexes)].ReleaseMutex();
		}

		private void Resize()
		{
			int oldCapacity = sizeOfTable;
			for (int i = 0; i < oldCapacity; i++)
			{
				locks[0, i].WaitOne();
			}

			try
			{
				if (sizeOfTable != oldCapacity)
				{
					return;
				}
				var oldTable = table;

				sizeOfTable = 2 * oldCapacity;
				table = new List<Node>[2, sizeOfTable];
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < sizeOfTable; j++)
					{
						table[i, j] = new List<Node>(sizeOfList);
					}
				}

				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < sizeOfTable; j++)
					{
						foreach (var node in oldTable[i, j])
						{
							Add(node.StudentID, node.CourseID);
						}
					}
				}
			}
			finally
			{
				for (int i = 0; i < oldCapacity; i++)
				{
					locks[0, i].ReleaseMutex();
				}
			}
		}
		private bool Relocate(long tableNum, long index)
		{
			long otherIndex = 0;
			long otherTableNum = 1 - tableNum;

			for (int round = 0; round < limit; round++)
			{
				var firstTable = table[tableNum, index];
				var tempNode = firstTable[0];

				switch (tableNum)
				{
					case 0:
						otherIndex = GetSecondHash(tempNode.StudentID, sizeOfTable);
						break;
					case 1:
						otherIndex = GetFirstHash(tempNode.StudentID, sizeOfTable);
						break;
				}

				Acquire(tempNode.StudentID);
				var otherTable = table[otherTableNum, otherIndex];

				try
				{
					if (firstTable.Remove(tempNode))
					{
						if (otherTable.Count < threshold)
						{
							otherTable.Add(tempNode);

							return true;
						}
						else if (otherTable.Count < sizeOfList)
						{
							otherTable.Add(tempNode);

							tableNum = 1 - tableNum;
							index = otherIndex;
							otherTableNum = 1 - otherTableNum;
						}
						else
						{
							firstTable.Add(tempNode);

							return false;
						}
					}
					else if (firstTable.Count >= threshold)
					{
						continue;
					}
					else
					{
						return true;
					}
				}
				finally
				{
					Release(tempNode.StudentID);
				}
			}
			return false;
		}

		public void Add(long studentID, long courseID)
		{
			Acquire(studentID);

			long firstIndex = GetFirstHash(studentID, sizeOfTable), secondIndex = GetSecondHash(studentID, sizeOfTable);
			long tableNum = -1, otherTableNum = -1;
			bool needResize = false;

			try
			{
				if (Contains(studentID, courseID))
				{
					return;
				}

				var tempNode = new Node(studentID, courseID);
				var firstTable = table[0, firstIndex];
				var secondTable = table[1, secondIndex];

				if (firstTable.Count < threshold)
				{
					firstTable.Add(tempNode);
					return;
				}
				else if (secondTable.Count < threshold)
				{
					secondTable.Add(tempNode);
					return;
				}
				else if (firstTable.Count < sizeOfList)
				{
					firstTable.Add(tempNode);
					tableNum = 0;
					otherTableNum = firstIndex;
				}
				else if (secondTable.Count < sizeOfList)
				{
					secondTable.Add(tempNode);
					tableNum = 1;
					otherTableNum = secondIndex;
				}
				else
				{
					needResize = true;
				}
			}
			finally
			{
				Release(studentID);
			}

			if (needResize)
			{
				Resize();
				Add(studentID, courseID);
			}
			else if (!Relocate(tableNum, otherTableNum))
			{
				Resize();
			}
			return;
		}

		public void Remove(long studentID, long courseID)
		{
			Acquire(studentID);

			try
			{
				var firstTable = table[0, GetFirstHash(studentID, sizeOfTable)];

				if (firstTable.Any(x => x.StudentID == studentID && x.CourseID == courseID))
				{
					firstTable.Remove(firstTable.Find(x => x.StudentID == studentID && x.CourseID == courseID));
					return;
				}
				else
				{
					var secondTable = table[1, GetSecondHash(studentID, sizeOfTable)];

					if (secondTable.Any(x => x.StudentID == studentID && x.CourseID == courseID))
					{
						secondTable.Remove(secondTable.Find(x => x.StudentID == studentID && x.CourseID == courseID));
						return;
					}
				}
				return;
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
				var firstTable = table[0, GetFirstHash(studentID, sizeOfTable)];

				if (firstTable.Any(x => x.StudentID == studentID && x.CourseID == courseID))
				{
					return true;
				}
				else
				{
					var secondTable = table[1, GetSecondHash(studentID, sizeOfTable)];

					if (secondTable.Any(x => x.StudentID == studentID && x.CourseID == courseID))
					{
						return true;
					}
				}

				return false;
			}
			finally
			{
				Release(studentID);
			}
		}
	}
}