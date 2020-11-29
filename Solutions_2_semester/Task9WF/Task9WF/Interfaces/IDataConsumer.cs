using System;

namespace Task9WF.Interfaces
{
	public interface IDataConsumer
	{
		event EventHandler<Message> NewInput;
		event EventHandler Stopped;

		TaskManager TaskManager { set; }

		bool Started { get; }
		int Port { get; set; }
		int RequestStartPort();
		void AddMessage(object sender, string message);
		void ChangeConnection(object endPoint, string name);
		void Start();
	}
}
