using System;
using System.Collections.Generic;
using System.Threading;

namespace ThirdTask
{
	public class Consumer
	{
		private Manager manager;

		private string name;
		private Thread thread;

		private volatile bool isFinished;

		public Consumer(string name, Manager mng)
		{
			manager = mng;

			this.name = name;

			isFinished = false;

			thread = new Thread(Remove);
			thread.Start();

		}

		private void Remove()
		{
			while (!isFinished)
			{
				manager.Remove(name);
				Thread.Sleep(50);
			}
		}

		public void Join()
		{
			isFinished = true;
			thread.Join();
		}
	}
}