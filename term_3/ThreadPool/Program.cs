using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
	class Program
	{
		public void Hello()
		{
			Console.WriteLine("Hello wrld");
		}
		static void Main(string[] args)
		{
			Program p = new Program();
			Action a = p.Hello;
			Console.Write("How many Threads will be: ");
			Interaction inter = new Interaction();
			int n = inter.GetInt();
			MyThreadPool myPool = new MyThreadPool(n);
			for  (int i = 0; i < 5; ++i)
				myPool.Enqueue(a);
			Thread.Sleep(100);
			myPool.Dispose();
			Console.ReadKey();
		}
	}
}
