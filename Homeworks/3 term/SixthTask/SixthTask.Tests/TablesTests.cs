using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HTLib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SixthTask.Tests
{
	public class TablesTests
	{
		[TestClass]
		public class SetsTest
		{
			int sizeOfTable = 1024;
			int tasksSize = 16;

			[TestMethod]
			public void ClosedTableTest()
			{
				var closedTable = new RefinableHT(sizeOfTable);
				var tasks = new List<Task>(tasksSize);

				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i] = new Task(() => RequestTo(closedTable));
				}
				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i].Start();
				}
				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i].Wait();
					tasks[i].Dispose();
				}
			}

			[TestMethod]
			public void CuckooTableTest()
			{
				var cuckooTable = new StripedCuckooHT(sizeOfTable);
				var tasks = new List<Task>(tasksSize);

				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i] = new Task(() => RequestTo(cuckooTable));
				}
				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i].Start();
				}
				for (int i = 0; i < tasks.Count; i++)
				{
					tasks[i].Wait();
					tasks[i].Dispose();
				}
			}

			private static void RequestTo(IExamSystem deanery)
			{
				for (int i = 0; i < 1200; i++)
				{
					long studentId = Guid.NewGuid().GetHashCode();
					long courseId = i;

					deanery.Add(studentId, courseId);
					Assert.IsTrue(deanery.Contains(studentId, courseId));

					deanery.Remove(studentId, courseId);
					Assert.IsFalse(deanery.Contains(studentId, courseId));
				}
			}
		}
	}
}