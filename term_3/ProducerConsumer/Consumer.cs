using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
	public class Consumer
	{
		private Thread myThread;
		private List<Data<string>> taskLst;
		private volatile bool stop;
		public Consumer(List<Data<string>> lst, string threadName)
		{
			myThread = new Thread(Work);
			myThread.Name = threadName;
			taskLst = lst;
			stop = false;
			myThread.Start();
		}
		private void Work()
		{
			bool lockStatus = false;
			while (!stop)
			{
				lockStatus = false;
				Monitor.Enter(taskLst, ref lockStatus);
				if (taskLst.Count > 0)
				{
					Data<string> dat = taskLst.First();
					taskLst.RemoveAt(0);
				}
				//Console.WriteLine($"{Thread.CurrentThread.Name}");
				Thread.Sleep(100);
				Monitor.Exit(taskLst);
			}
		}
		private void Stop()
		{
			stop = true;
		}
		public void Join()
		{
			Stop();
			myThread.Join();
		}
	}
}
