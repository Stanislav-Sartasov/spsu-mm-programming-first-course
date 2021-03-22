using System;
using System.Collections.Generic;
using System.Threading;

namespace ThirdTask
{
	public class Producer
	{
		private Manager manager;

		private string name;
		private Thread thread;

		private volatile bool isFinished;

		public Producer(string name, Manager mng)
		{

			manager = mng;

			this.name = name;

			isFinished = false;

			thread = new Thread(Add);
			thread.Start();
		}

		private void Add()
		{
			while (!isFinished)
			{
				manager.Add(name);
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