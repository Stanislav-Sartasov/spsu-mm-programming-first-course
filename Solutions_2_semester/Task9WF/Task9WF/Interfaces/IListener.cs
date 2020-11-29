using System;
using System.Net.Sockets;

namespace Task9WF.Interfaces
{
	public interface IListener
	{
		event EventHandler<Socket> NewConnection;
		bool Started { get; }
		Info MyInfo { get; }
		TaskManager TaskManager { set; }
		IMessager Messager { set; }
		bool Init();
		bool Init(int startPort);
		bool Init(int[] portsList);
		bool Init(int portMin, int portMax);
		void Start(int backlog);
		void Stop();
	}
}
