using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

namespace NinthTask.ChatDescription
{
	class Server
	{
		private bool isServerAlive;
		public Dictionary<string, Socket> SockerUserList; // список пользователей (адрес/имя, сокет)
		public Mutex MutexSocketList; // мьютекс на доступ к словарю
		public Mutex MutexSocketServer; // мьютекс на доступ к сокету сервера

		Socket Listener;
		Thread ThreadListenForNewUsers;
		Thread ThreadListenForClientsDataReceiving;

		void StartServer()
		{
			IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);

			Listener = new Socket(AddressFamily.InterNetwork,
				SocketType.Stream, ProtocolType.Tcp);

			while (true)
			{
				try
				{
					Listener.Bind(localEndPoint);
				}
				catch (Exception e)
				{
					localEndPoint.Port += 1;
					continue;

				}
				break;
			}

			Console.WriteLine("Address Server: " + localEndPoint.ToString());
		}

		void StartListeningForNewUsers() // Listening for new users
		{
			try
			{
				Listener.Listen(100);

				while (isServerAlive)
				{
					Socket handler = Listener.Accept();
					var bytes = new byte[1024];
					int bytesRec = handler.Receive(bytes);
					if (bytesRec == 4 && BitConverter.ToInt32(bytes, 0) == 0)
					{
						MutexSocketList.WaitOne();
						SockerUserList.Add(handler.RemoteEndPoint.ToString(), handler);
						Console.WriteLine("New User: " + handler.RemoteEndPoint.ToString());
						MutexSocketList.ReleaseMutex();
					}
					else
					{
						handler.Close();
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Disconnected.");
			}
		}

		void ClientDataReceiving() // Client data receiving
		{
			while (isServerAlive)
			{
				MutexSocketList.WaitOne();
				var iRemove = SockerUserList.Where(f => f.Value.Connected == false).ToArray();
				foreach (var i in iRemove)
				{
					SockerUserList.Remove(i.Key);
				}

				foreach (var pair in SockerUserList)
				{
					var socketUser = pair.Value;

					// если пришедших байтов больше 4
					if (socketUser.Available > 4)
					{
						byte[] bdata = new byte[1024];
						int bsize = socketUser.Receive(bdata);
						int code = BitConverter.ToInt32(bdata, 0);
						if (code == 2)
						{
							int len = BitConverter.ToInt32(bdata, 4);
							string data = Encoding.ASCII.GetString(bdata, 8, len);
							Console.WriteLine("{0}> {1}", pair.Key, data);
							SendOthers(pair.Key, data, false);
						}
						if (code == 3)
						{
							int len = BitConverter.ToInt32(bdata, 4);
							string data = Encoding.ASCII.GetString(bdata, 8, len);
							Console.WriteLine(data);
							ForwardOther(bdata, 8 + len, pair.Key);
						}
					}
				}
				MutexSocketList.ReleaseMutex();
				Thread.Sleep(0);
			}
		}

		void SendOthers(string name, string message, bool onlyUsers)
		{
			var bytes = new byte[1024];

			BitConverter.GetBytes(3).CopyTo(bytes, 0);
			string sMessage = name + "> " + message;
			BitConverter.GetBytes(sMessage.Length).CopyTo(bytes, 4);
			Encoding.ASCII.GetBytes(sMessage).CopyTo(bytes, 8);

			int sizeOfBytes = 8 + sMessage.Length;

			if (onlyUsers)
			{
				foreach (var pair in SockerUserList)
				{
					try
					{
						var socket = pair.Value;
						socket.Send(bytes, sizeOfBytes, SocketFlags.None);
					}
					catch (Exception e)
					{
						throw new Exception(e.Message);
					}
				}
			}
			else
			{
				foreach (var pair in this.SockerUserList)
				{
					if (pair.Key != name)
					{
						try
						{
							var socket = pair.Value;
							socket.Send(bytes, sizeOfBytes, SocketFlags.None);
						}
						catch (Exception e)
						{
							throw new Exception(e.Message);
						}
					}
				}
			}
		}
		void ForwardOther(byte[] bytes, int sizeOfBytes, string name = null, bool onlyUsers = false)
		{
			if (onlyUsers)
			{
				foreach (var pair in SockerUserList)
				{
					try
					{
						var socket = pair.Value;
						socket.Send(bytes, sizeOfBytes, SocketFlags.None);
					}
					catch (Exception e)
					{
						throw new Exception(e.Message);
					}
				}
			}
			else
			{
				foreach (var pair in SockerUserList)
				{
					if (pair.Key != name)
					{
						try
						{
							var socket = pair.Value;
							socket.Send(bytes, sizeOfBytes, SocketFlags.None);
						}
						catch (Exception e)
						{
							throw new Exception(e.Message);
						}
					}
				}
			}
		}

		public void Start()
		{
			isServerAlive = true;
			StartServer();

			ThreadListenForNewUsers = new Thread(StartListeningForNewUsers);
			ThreadListenForNewUsers.Start();

			ThreadListenForClientsDataReceiving = new Thread(ClientDataReceiving);
			ThreadListenForClientsDataReceiving.Start();

			Console.WriteLine("type 'exit' to stop server");

			while (true)
			{
				var input = Console.ReadLine();
				if (input == "exit")
				{
					Abort();
					break;
				}
			}
		}
		public void Abort()
		{
			ThreadListenForNewUsers.Suspend();
			ThreadListenForClientsDataReceiving.Suspend();
			// ThreadListenForNewUsers.Join();
			// ThreadListenForClientsDataReceiving.Join();
		}

		public Server()
		{
			MutexSocketList = new Mutex();
			MutexSocketServer = new Mutex();
			SockerUserList = new Dictionary<string, Socket>();
		}
	}
}