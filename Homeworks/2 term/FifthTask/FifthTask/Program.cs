using System;
using System.Threading;
using FifthTask.WTreeDescription;

namespace FifthTask
{
	class Program
	{
		static void Main()
		{
			// This is a little sample of program's not crashing; checking tests are performed in the unit test.
			var tree = new WTree<MyTestClass>(time: 1200);

			tree.AddElement(1, new MyTestClass(10));
			tree.AddElement(2, new MyTestClass(20));
			tree.AddElement(5, new MyTestClass(30));
			tree.AddElement(2, new MyTestClass(40));
			tree.AddElement(4, new MyTestClass(62));
			tree.AddElement(3, new MyTestClass(48));

			tree.RemoveElement(4);
			tree.RemoveElement(5);

			Thread.Sleep(3000);
			GC.Collect();
		}
	}
}
