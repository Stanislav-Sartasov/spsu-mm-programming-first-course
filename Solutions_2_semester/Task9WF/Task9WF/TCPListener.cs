using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Task9WF.Interfaces;

namespace Task9WF.Multithreaded
{
	public class TCPListener : IListener
	{
		bool inited = false;
		public bool Started { get; private set; } = false;
		public event EventHandler<Socket> NewConnection;
		TaskManager tasks = null;
		Info myInfo = null;
		Socket socket = null;
		object locker = new object();
		IMessager messager;
		public IMessager Messager
		{
			set
			{
				lock (locker)
				{
					if (messager == null)
						messager = value;
				}
			}
		}

		public Info MyInfo
		{
			get
			{
				lock (locker)
				{
					return myInfo;
				}
			}
		}

		public TaskManager TaskManager
		{
			set
			{
				lock (locker)
				{
					if (tasks == null)
						tasks = value;
				}
			}
		}

		public bool Init()
		{
			return Init(49152, 65535);
		}
		public bool Init(int startPort)
		{
			if (startPort < 0)
				return Init();
			else
				return Init(new int[] { startPort });
		}
		public bool Init(int portMin, int portMax)
		{
			lock (locker)
			{
				if (Started)
					return false;
			}

			List<int> portsList = new List<int> { portMin };
			for (int i = portMin + 1; i < portMax; i++)	 //like in Random.Next()
				portsList.Add(i);

			return Init(portsList.ToArray());
		}
		public bool Init(int[] portsList)
		{
			lock (locker)
			{
				if (Started)
					return false;

				if (messager == null || tasks == null)
				{
					inited = false;
					return false;
				}

				IPAddress[] ipAddressesList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

				foreach (int port in portsList)
				{
					try
					{
						socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
						socket.Bind(new IPEndPoint(IPAddress.Any, port));

						myInfo = new Info
						{
							Guid = Guid.NewGuid(),
							Port = port
						};

						List<IPAddress> interNetworkAddresses = new List<IPAddress>();
						foreach (IPAddress iPAddress in ipAddressesList)
							if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
								interNetworkAddresses.Add(iPAddress);
						interNetworkAddresses.Add(IPAddress.Parse("127.0.0.1"));
						myInfo.IPAddresses = interNetworkAddresses.ToArray();

						break;
					}
					catch
					{
						try
						{
							socket.Close();
						}
						catch { }
						myInfo = null;
					}
				}

				if (myInfo == null)
				{
					inited = false;
					return false;
				}

				inited = true;
				return true;
			}
		}
		public async void Start(int backlog)
		{
			lock (locker)
			{
				if (!inited)
					return;

				Started = true;
				socket.Listen(backlog);
			}

			await tasks.Run(() =>
			{
				while (true)
					try
					{
						Socket newSocket = socket.Accept();
						NewConnection(this, newSocket);
					}
					catch
					{
						if (!Started)
							return;
					}
			});
		}
		
		public void Stop()
		{
			socket.Close();
			Started = false;
		}
	}
}