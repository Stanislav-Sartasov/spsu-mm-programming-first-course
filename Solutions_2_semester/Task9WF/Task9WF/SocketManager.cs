using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using Task9WF.Interfaces;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace Task9WF.Multithreaded
{
	public class SocketManager : IConnectionManager
	{
		public event EventHandler<string> NewMessage;
		public event EventHandler<string> SystemMessage;
		public event EventHandler<string> ChangeConnection;

		TaskManager tasks = null;
		Info myInfo = null;
		IMessager messager = null;
		string name = null;
		bool started = false;
		bool stopped = false;
		object constDataLocker = new object();

		object addConnectionLocker = new object();

		enum AddConnectionKodes
		{
			Success,
			Fail,
			Reconnect
		}

		const int StandartTimeOut = 25000;
		int timeOut = -1;
		public int TimeOut 
		{ 
			get
			{
				if (timeOut < 0)
					return StandartTimeOut;
				return timeOut;
			}
			set
			{
				timeOut = value;
			}
		}

		public TaskManager TaskManager
		{
			set
			{
				lock (constDataLocker)
				{
					if (tasks == null)
						tasks = value;
				}
			}
		}
		public Info MyInfo
		{
			set
			{
				lock (constDataLocker)
				{
					if (myInfo == null)
						myInfo = value;
				}
			}
		}
		public IMessager Messager
		{
			set
			{
				lock (constDataLocker)
				{
					if (messager == null)
						messager = value;
				}
			}
		}
		public string Name
		{
			get
			{
				lock (constDataLocker)
				{
					return name == null ? "???" : name;
				}
			}
			set
			{
				lock (constDataLocker)
				{
					if (name == null)
					{
						name = value;
						tasks.Run(() => SendToAll(MessageType.Name, Name));
					}
				}
			}
		}
		public bool Started
		{
			get
			{
				lock (constDataLocker)
				{
					return started;
				}
			}
		}

		Hashtable connections = new Hashtable();
		object connectionsLocker = new object();

		public void Start()
		{
			lock (constDataLocker)
			{
				if (!started)
					if (myInfo != null && messager != null && tasks != null)
					{
						lock (constDataLocker)
						{
							started = true;
						}
					}
			}
		}

		public void AddConnection(object sender, Socket socket)
		{
			AddConnectionAdvanced(sender, socket);
		}

		AddConnectionKodes AddConnectionAdvanced(object sender, Socket socket)
		{
			IPEndPoint remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
			Info newConnectionInfo = null;
			Connection connection = null;
			Connection oldConnection = null;
			bool needReconnect = false;
			bool larger = false;
			while (true)
			{
				lock (constDataLocker)
				{
					if (stopped)
						return AddConnectionKodes.Fail;
					if (started)
						break;
				}
				Thread.Sleep(250);
			}

			try
			{
				if (!messager.SendMessage(socket, MessageType.Socket, myInfo.ToString()))
					throw new Exception();
				Message message = messager.ReceiveMassege(socket);
				if (message.Type != (byte)MessageType.Socket)
					throw new Exception();
				newConnectionInfo = Info.Parse(message.Text);

				if (newConnectionInfo == null)
					throw new Exception();

				if (newConnectionInfo.Guid.Equals(myInfo.Guid))
					throw new Exception();

				lock (addConnectionLocker)
				{
					larger = Larger(newConnectionInfo.Guid.ToByteArray(), myInfo.Guid.ToByteArray());
					if (sender == this)
					{
						if (larger)
						{
							StopSocket(socket);
							return AddConnectionKodes.Reconnect;
						}
					}
					else if (!larger)
					{
						StopSocket(socket);
						needReconnect = true;
						throw new Exception();
					}

					lock (connectionsLocker)
					{
						foreach (Guid key in connections.Keys)
							if (key.Equals(newConnectionInfo.Guid))
							{
								oldConnection = (Connection)connections[newConnectionInfo.Guid];
								if (!oldConnection.Connected)
									oldConnection.Close();
								if (!oldConnection.Closed)
									throw new Exception();
								oldConnection.Socket = socket;
								break;
							}

						if (oldConnection == null)
						{
							connection = new Connection
							{
								Info = newConnectionInfo,
								Socket = socket
							};
							connections.Add(newConnectionInfo.Guid, connection);
						}
					}

					SendToAll(MessageType.Socket, newConnectionInfo.ToString());
				}
			}
			catch
			{
				if (!needReconnect)
				{
					StopSocket(socket);
					return AddConnectionKodes.Fail;
				}
			}

			if (needReconnect)
			{
				tasks.Run(() => TryConnect(newConnectionInfo));
				return AddConnectionKodes.Reconnect;
			}

			if (connection != null)
			{
				ChangeConnection(connection.Info.Guid, connection.Name);
				tasks.Run(() => SocketHandling(connection, !larger));
			}
			else
				ChangeConnection(oldConnection.Info.Guid, oldConnection.Name);
			return AddConnectionKodes.Success;
		}

		bool Larger(byte[] a, byte[] b)
		{
			if (a.Length > b.Length)
				return true;
			else if (b.Length > a.Length)
				return false;

			for (int i = 0; i < a.Length; i++)
				if (a[i] > b[i])
					return true;
				else if (a[i] < b[i])
					return false;

			return false;
		}

		void StopSocket(Socket socket)
		{
			try
			{
				socket.Close();
			}
			catch { }
		}

		void SocketHandling(Connection connection, bool reconnect)
		{
			void Stop()
			{
				lock (connectionsLocker)
				{
					connection.Close();
					connections.Remove(connection);
				}
			}

			bool connectionAccepted = false;
			while (true)
			{
				messager.SendMessage(connection.Socket, MessageType.Name, Name);

				if (stopped)
				{
					Stop();
					return;
				}

				lock (connectionsLocker)
				{
					if (!connection.Connected)
						connection.Close();
				}

				if (!connection.Connected && reconnect)
					TryConnect(connection.Info);

				for (int i = 0; !connection.Connected && i < TimeOut / 250; i++)
				{
					if (stopped)
					{
						Stop();
						return;
					}
					Thread.Sleep(250);
				}

				if (!connection.Connected)
				{
					Stop();
					return;
				}

				Message message;
				while (!stopped && connection.Connected && !connection.Closed)
				{
					message = messager.ReceiveMassege(connection.Socket);
					if (message.Type == (byte)MessageType.Error)
						break;

					switch (message.Type)
					{
						case (byte)MessageType.Message:
							NewMessage(connection.Info.Guid, message.Text);
							break;
						case (byte)MessageType.Name:
							connection.Name = message.Text;
							ChangeConnection(connection.Info.Guid, connection.Name);
							break;
						case (byte)MessageType.Socket:
							tasks.Run(() => TryConnect(Info.Parse(message.Text)));
							break;
					}

					if (!connectionAccepted)
					{
						SystemMessage(this, connection.Name + " connected");
						connectionAccepted = true;
					}
				}

				Thread.Sleep(250);

				lock (constDataLocker)
				{
					if (!stopped)
					{
						SystemMessage(this, connection.Name + " lost connection");
						connectionAccepted = false;
						ChangeConnection(connection.Info.Guid, null);
					}
					else
					{
						Stop();
						return;
					}
				}
			}
		}
		void TryConnect(Info info)
		{
			if (info == null)
				return;
			lock (constDataLocker)
			{
				if (stopped)
					return;
				if (myInfo.Guid.Equals(info.Guid))
					return;
			}
			lock (connectionsLocker)
			{
				foreach (Guid guid in connections.Keys)
					if (guid.Equals(info.Guid))
						return;
			}
			foreach (var address in info.IPAddresses)
			{
				var key = TryConnect(new IPEndPoint(address, info.Port));
				if (key == null)
					return;
				if (key == "")
					return;
			}
		}
		public string TryConnect(string address, int port)
		{
			try
			{
				return TryConnect(new IPEndPoint(IPAddress.Parse(address), port));
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public string TryConnect(EndPoint remoteEndPoint)
		{
			IPEndPoint endPoint = (IPEndPoint)remoteEndPoint;
			Socket socket;

			lock (constDataLocker)
			{
				if (stopped)
					return "";

				foreach (var localAddress in myInfo.IPAddresses)
					if (localAddress.Equals(endPoint))
						return "Already connected";
			}

			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				var task = tasks.Run(() =>
				{
					try
					{
						socket.Connect(remoteEndPoint);
					}
					catch { }
				});
				if (!task.Wait(TimeSpan.FromMilliseconds(TimeOut)))
				{
					socket.Close();
					throw new Exception();
				}
			}
			catch
			{
				return "Fail connection";
			}

			if (AddConnectionAdvanced(this, socket) == AddConnectionKodes.Fail)
				return "Fail connection";

			return null;
		}

		public void SendToAll(MessageType type, string message)
		{
			lock (constDataLocker)
			{
				if (stopped)
					return;
			}
			lock (connectionsLocker)
			{
				if (connections.Count == 0)
					return;
				foreach (Connection connection in connections.Values)
					if (!messager.SendMessage(connection.Socket, type, message))
						connection.AddMessage(new Message() { Type = (byte)type, Text = message });
			}
		}

		public void Stop()
		{
			lock (constDataLocker)
			{
				stopped = true;
			}
			lock (connectionsLocker)
			{
				foreach (Connection connection in connections.Values)
					connection.Close();
			}
		}
	}
}
