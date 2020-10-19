using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
	public class Producer
	{
		private Thread myThread;
		private List<Data<string>> taskLst;
		private volatile bool stop;
		public Producer(List<Data<string>> lst, string threadName)
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
			string i = Thread.CurrentThread.Name;
			while (!stop)
			{
				lockStatus = false;
				Monitor.Enter(taskLst, ref lockStatus);
				taskLst.Add(new Data<string>(i));
				//Console.WriteLine($"{Thread.CurrentThread.Name}");
				Thread.Sleep(100);
				Monitor.Exit(taskLst);
			}

		}
		public void Stop()
		{
			stop = true;
		}
		public void Join()
		{
			myThread.Join();
		}
	}
}
