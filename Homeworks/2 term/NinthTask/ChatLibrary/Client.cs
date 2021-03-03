using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatDescription
{
	public class Client
	{
		public List<IPEndPoint> UserList { get; private set; } //private??
		public IPEndPoint ClientIP { get; private set; }
		internal int IsConnected { get; private set; }
		private readonly Socket Listener;
		private readonly Mutex MutexUserList;
		private readonly Mutex MutexListener;

		internal string GetInput()
		{
			return Console.ReadLine().Trim();
		}

		public void TryGetAddress(IPEndPoint address)
		{
			try
			{
				Listener.Bind(address);

				ClientIP = address;
				IsConnected = 1;

				Console.WriteLine($">Your ip is {ClientIP.Address}:{ClientIP.Port}.");
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void SendMessage(byte[] msg, IPEndPoint startChannel = null)
		{
			if (startChannel != null) // First connection to another client
			{
				Listener.SendTo(msg, startChannel);
				Console.WriteLine("Setting connection!");
			}
			else // Connection to already connected clients
			{
				Console.WriteLine("Broadcasting!");
				foreach (var ip in UserList)
				{
					Listener.SendTo(msg, ip);
				}
			}
		}

		public void Connect(IPEndPoint ip)
		{
			if (ip.Equals(ClientIP))
			{
				Console.WriteLine(">Error! This is your ip.");
			}
			else
			{
				if (UserList.Contains(ip))
				{
					Console.WriteLine(">Error! You are already connected.");
				}
				else
				{
					SendMessage(ByteOperations.MessageToBytes("+", ClientIP, UserList), ip);
					UserList.Add(ip);
				}
			}
		}

		public void Disconnect()
		{
			MutexUserList.WaitOne();
			if (IsConnected != 2)
			{
				if (IsConnected == 1)
				{
					Console.WriteLine(">No active connections found!");
				}
			}
			else
			{
				SendMessage(ByteOperations.MessageToBytes("-", ClientIP));
				UserList.Clear();
			}
			MutexUserList.ReleaseMutex();
		}

		public void Exit()
		{
			Disconnect();

			Listener.Shutdown(SocketShutdown.Both);
			Listener.Close();

			Environment.Exit(0);
		}

		public void StartListening()
		{
			var listen = new Thread(new ThreadStart(Listen));
			listen.Start();
		}

		private void Listen() //внимательно изучить - в парсерах ошибки
		{
			try
			{
				while (true)
				{
					int checkBytes = 0;

					var buffer = new byte[1024];
					var message = new List<byte>();
					EndPoint sender = new IPEndPoint(IPAddress.Any, 0);

					//Console.WriteLine("Handling..");
					MutexListener.WaitOne();
					//пересмотреть??
					do
					{
						checkBytes += Listener.ReceiveFrom(buffer, ref sender);
						message.AddRange(buffer);
					}
					while (Listener.Available > 0);
					message.RemoveRange(checkBytes, message.Count - checkBytes);
					//
					MutexListener.ReleaseMutex();

					MutexUserList.WaitOne();
					switch ((char)message[0])
					{
						case '0':
							var senderIP = sender as IPEndPoint;
							string recMsg = ByteOperations.MessageFromBytes(message, 1);

							Console.WriteLine($">({senderIP.Address}:{senderIP.Port}): {recMsg}");
							break;
						case '+': //connect
							Console.WriteLine("Somebody joined?");
							List<IPEndPoint> recIPs = ByteOperations.MessageFromBytes(message, 0);

							var newConnection = false;
							foreach (var ip in recIPs) //someone has joined! //здесь ошибка при подключении третьего
							{
								Console.WriteLine("from" + ip);
								if (!UserList.Contains(ip))
								{
									UserList.Add(ip);
									newConnection = true;
								}
							}

							IsConnected = 2;

							if (newConnection)
							{
								Console.WriteLine(">New user joined, now your connections are:");

								foreach (var ip in UserList)
								{
									Console.WriteLine($">>{ip}");

									var temp = new List<IPEndPoint>(UserList);
									temp.Remove(ip);

									SendMessage(ByteOperations.MessageToBytes("+", ClientIP, temp), ip);
								}
							}
							break;
						case '-': //disonnect
							recIPs = ByteOperations.MessageFromBytes(message, 0);

							UserList.Remove(recIPs[0]);
							Console.WriteLine($">{recIPs[0]} disconnected");

							break;
					}

					if (UserList.Count == 0)
					{
						IsConnected = 1;
					}
					MutexUserList.ReleaseMutex();
				}
			}
			catch(Exception ex) //SocketException
			{
				//Console.WriteLine(">Error! Please, check your connection.");
				Console.WriteLine(ex.ToString());
				Listen();
			}
		}

		public Client()
		{
			UserList = new List<IPEndPoint>();
			Listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			IsConnected = 0;

			MutexUserList = new Mutex();
			MutexListener = new Mutex();

			Listener.IOControl(-1744830452, new byte[] { 0 }, null);
		}
	}
}