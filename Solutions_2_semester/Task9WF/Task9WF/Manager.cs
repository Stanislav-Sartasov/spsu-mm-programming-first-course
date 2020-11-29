using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Task9WF.Interfaces;

namespace Task9WF
{
	public class Manager
	{ 
		IDataConsumer dataConsumer;
		IListener listener;
		IConnectionManager connectionManager;
		IMessager messager;
		TaskManager taskManager;
		TaskManager.StopDelegate taskManagerStop;

		public bool Active { get; private set; }

		const string SystemMessagePrefix = "#";
		void SystemMessage(object sender, string message)
		{
			dataConsumer.AddMessage(this, SystemMessagePrefix + " " + message);
		}
		public Guid Start(IDataConsumer dataConsumerObj, IListener listenerObj, IConnectionManager connectionManagerObj, IMessager messagerObj)
		{
			taskManager = new TaskManager(out taskManagerStop);
			dataConsumer = dataConsumerObj;
			listener = listenerObj;
			connectionManager = connectionManagerObj;
			messager = messagerObj;

			dataConsumer.TaskManager = taskManager;
			dataConsumer.NewInput += NewInput;
			dataConsumer.Stopped += Stop;

			Task.Run(() => dataConsumerObj.Start());

			while (!dataConsumerObj.Started)
				Thread.Sleep(500);

			connectionManager.TaskManager = taskManager;
			connectionManager.NewMessage += dataConsumer.AddMessage;
			connectionManager.ChangeConnection += dataConsumer.ChangeConnection;
			connectionManager.SystemMessage += SystemMessage;
			connectionManager.Messager = messager;

			listener.TaskManager = taskManager;
			listener.NewConnection += connectionManager.AddConnection;
			listener.Messager = messager;

			while (!listener.Init(dataConsumerObj.RequestStartPort())) ;
			listener.Start(1024);

			connectionManager.TaskManager = taskManager;
			connectionManager.MyInfo = listener.MyInfo;
			connectionManager.Start();

			SystemMessage(this, "Server started at port: " + listener.MyInfo.Port.ToString());
			dataConsumer.Port = listener.MyInfo.Port;
			Active = true;

			return listener.MyInfo.Guid;
		}

		void NewInput(object sender, Message input)
		{
			switch (input.Type)
			{
				case (byte)MessageType.Message:
					connectionManager.SendToAll(MessageType.Message, input.Text);
					SystemMessage(this, "Message send: " + input.Text);
					break;
				case (byte)MessageType.Name:
					connectionManager.Name = input.Text;
					SystemMessage(this, "Name set: " + connectionManager.Name);
					break;
				case (byte)MessageType.Socket:
					EndPoint endPoint;

					SystemMessage(this, "Trying connect to: " + input.Text);
					endPoint = input.GetAddress();

					if (endPoint == null)
					{
						SystemMessage(this, "Can't connect to: " + input.Text + " - Wrong address");
						break;
					}

					connectionManager.TryConnect(endPoint);
					break;
				default:
					SystemMessage(this, input.Text);
					break;
			}
		}
		void Stop(object sender, EventArgs args)
		{
			listener.Stop();
			connectionManager.Stop();
			Task.Run(() => taskManagerStop());
			Thread.Sleep(5000);
			Active = false;
		}
	}
}
