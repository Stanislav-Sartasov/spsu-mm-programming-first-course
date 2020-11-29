using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Task9WF.Multithreaded;

namespace Task9WF
{
	class Program
	{
		[STAThread]
		static void Main()
		{
			Manager manager = new Manager();
			manager.Start(new FormHandler(), new TCPListener(), new SocketManager(), new Messager());

			while (manager.Active)
				Thread.Sleep(1000);
		}
	}
}
