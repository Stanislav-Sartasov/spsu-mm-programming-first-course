using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NinthTask.ChatDescription
{
	class Chat
	{
		public static bool FlagWorking = true; // флаг работы
		public static Dictionary<string, Socket> SocketList = new Dictionary<string, Socket>(); // список пользователей (адрес/имя, сокет)
		public static Socket Listener;
		public static Socket SocketServer = null; // ServerAdress
		public static Mutex MutexSocketList = new Mutex(); // мьютекс на доступ к словарю
		public static Mutex MutexSocketServer = new Mutex(); // мьютекс на доступ к сокету сервера
		public static void StartListening() // Listening for new users
		{
			var localEndPoint = new IPEndPoint(IPAddress.Any, 11000);

			Listener = new Socket(AddressFamily.InterNetwork,
			    SocketType.Stream, ProtocolType.Tcp);
			while (true)
			{
				try
				{
					Listener.Bind(localEndPoint);
				}
				catch (Exception)
				{
					localEndPoint.Port += 1;
					continue;

				}
				break;
			}

			Console.WriteLine("Address Server: " + localEndPoint.ToString());

			try
			{
				Listener.Listen(100);

				while (FlagWorking)
				{
					var handler = Listener.Accept();
					var bytes = new byte[1024];
					int bytesRec = handler.Receive(bytes);
					if (bytesRec == 4 && BitConverter.ToInt32(bytes, 0) == 0)
					{
						MutexSocketList.WaitOne();
						SocketList.Add(handler.RemoteEndPoint.ToString(), handler);
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
				throw new Exception(e.Message);
			}
		}

		public static void SendServer(byte[] message, int size) // Sending message to server (if it exists) and all users
		{
			MutexSocketServer.WaitOne();

			if (SocketServer != null && SocketServer.Connected == true)
			{
				try
				{
					SocketServer.Send(message, size, SocketFlags.None);
				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
			}

			MutexSocketServer.ReleaseMutex();
			MutexSocketList.WaitOne();

			foreach (var i in SocketList)
			{
				try
				{
					i.Value.Send(message, size, SocketFlags.None);
				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
			}

			MutexSocketList.ReleaseMutex();
		}
		public static void SendOnlyUsers(string name, string message)
		{
			var bytes = new byte[1024];

			BitConverter.GetBytes(3).CopyTo(bytes, 0);
			string sMessage = name + "> " + message;
			BitConverter.GetBytes(sMessage.Length).CopyTo(bytes, 4);
			Encoding.ASCII.GetBytes(sMessage).CopyTo(bytes, 8);

			int sizeOFBytes = 8 + sMessage.Length;

			foreach (var i in SocketList)
			{
				try
				{
					i.Value.Send(bytes, sizeOFBytes, SocketFlags.None);
				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
			}
		}
		public static void SendOthers(string name, string message)
		{
			byte[] bytes = new byte[1024];

			BitConverter.GetBytes(3).CopyTo(bytes, 0);
			string sMessage = name + "> " + message;
			BitConverter.GetBytes(sMessage.Length).CopyTo(bytes, 4);
			Encoding.ASCII.GetBytes(sMessage).CopyTo(bytes, 8);

			int sizeOfBytes = 8 + sMessage.Length;

			foreach (var i in SocketList)
			{
				if (i.Key != name)
				{
					try
					{
						i.Value.Send(bytes, sizeOfBytes, SocketFlags.None);
					}
					catch (Exception e)
					{
						throw new Exception(e.Message);
					}
				}
			}

			MutexSocketServer.WaitOne();

			if (SocketServer != null && SocketServer.Connected == true)
			{
				SocketServer.Send(bytes, sizeOfBytes, SocketFlags.None);
			}

			MutexSocketServer.ReleaseMutex();
		}
		public static void ForwardOnlyUsers(byte[] bytes, int sizeOfBytes)
		{
			foreach (var i in SocketList)
			{
				try
				{
					i.Value.Send(bytes, sizeOfBytes, SocketFlags.None);
				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
			}
		}
		public static void ForwardOther(string name, byte[] bytes, int sizeOfBytes)
		{
			foreach (var i in SocketList)
			{
				if (i.Key != name)
				{
					try
					{
						i.Value.Send(bytes, sizeOfBytes, SocketFlags.None);
					}
					catch (Exception e)
					{
						throw new Exception(e.Message);
					}
				}
			}
			MutexSocketServer.WaitOne();
			if (SocketServer != null && SocketServer.Connected == true)
			{
				SocketServer.Send(bytes, sizeOfBytes, SocketFlags.None);
			}
			MutexSocketServer.ReleaseMutex();
		}
		public static void GetRead() // Client data receiving
		{
			while (FlagWorking)
			{
				MutexSocketList.WaitOne();
				var iRemove = SocketList.Where(f => f.Value.Connected == false).ToArray();
				foreach (var i in iRemove)
				{
					SocketList.Remove(i.Key);
				}

				foreach (var i in SocketList)
				{
					if (i.Value.Available > 4)
					{
						byte[] bdata = new byte[1024];
						int bsize = i.Value.Receive(bdata);
						int code = BitConverter.ToInt32(bdata, 0);
						if (code == 2)
						{
							int len = BitConverter.ToInt32(bdata, 4);
							string data = Encoding.ASCII.GetString(bdata, 8, len);
							Console.WriteLine("{0}> {1}", i.Key, data);
							SendOthers(i.Key, data);
						}
						if (code == 3)
						{
							int len = BitConverter.ToInt32(bdata, 4);
							string data = Encoding.ASCII.GetString(bdata, 8, len);
							Console.WriteLine(data);
							ForwardOther(i.Key, bdata, 8 + len);
						}
					}
				}
				MutexSocketList.ReleaseMutex();
				Thread.Sleep(0);
			}
		}
		public static void GetReadServer() // Server data monitoring
		{
			while (FlagWorking)
			{
				MutexSocketServer.WaitOne();
				if (SocketServer.Available > 0)
				{
					byte[] bytes = new byte[1024];
					//int socketGet = SocketServer.Receive(bytes);
					int code = BitConverter.ToInt32(bytes, 0);
					if (code >= 3)
					{
						int len = BitConverter.ToInt32(bytes, 4);
						string data = Encoding.ASCII.GetString(bytes, 8, len);
						Console.WriteLine(data);

						ForwardOnlyUsers(bytes, 8 + len);
					}
					if (code == 2)
					{
						int len = BitConverter.ToInt32(bytes, 4);
						string data = Encoding.ASCII.GetString(bytes, 8, len);
						Console.WriteLine("{0}> {1}", SocketServer.RemoteEndPoint.ToString(), data);

						SendOnlyUsers(SocketServer.RemoteEndPoint.ToString(), data);
					}
				}
				MutexSocketServer.ReleaseMutex();
				Thread.Sleep(0);
			}
		}
		public static void Connect(string address, int port)
		{
			SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//string address = "127.0.0.1";
			var ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
			SocketServer.Connect(ipPoint);
			Console.WriteLine("Address Local: " + SocketServer.LocalEndPoint.ToString());
			SocketServer.Send(BitConverter.GetBytes(0));
		}

		public void Start(string[] args)
		{
			var threads = new List<Thread>();

			//Server Part
			var threadList = new Thread(StartListening);
			threadList.Start();
			threads.Add(threadList);

			//Client reading part
			var threadRead = new Thread(GetRead);
			threadRead.Start();
			threads.Add(threadRead);

			//Server connection
			if (args.Length > 0)
			{
				try
				{
					if (args.Length == 1)
					{
						Connect("127.0.0.1", int.Parse(args[0]));
					}
					else if (args.Length == 2)
					{
						Connect(args[0], int.Parse(args[1]));
					}
					else 
					{
						throw new Exception("Wrong input"); // сделать, пока не будет правильный ввод
					}

					var threadReadServer = new Thread(GetReadServer);
					threadReadServer.Start();
					threads.Add(threadReadServer);
				}
				catch (Exception e)
				{
					if (((SocketException)e).SocketErrorCode == SocketError.ConnectionRefused ||
					    ((SocketException)e).SocketErrorCode == SocketError.TimedOut)
					{
						Console.WriteLine("Connection error: " + (args.Length == 2 ? args[0] + ":" + args[1] : "127.0.0.1:" + args[0]));
					}
				}
			}
			else
			{
				throw new ArgumentException("Can't read the adress");
			}

			while (true)
			{
				//waiting
				string s = Console.ReadLine();

				if (s == "exit") //exit
				{
					Listener.Close();

					FlagWorking = false;
					foreach (var i in threads)
					{
						i.Join();
					}
					break;
				}

				//coding string
				byte[] msgs = new byte[1024];
				BitConverter.GetBytes(2).CopyTo(msgs, 0);
				BitConverter.GetBytes(s.Length).CopyTo(msgs, 4);
				Encoding.ASCII.GetBytes(s).CopyTo(msgs, 8);
				//

				//отправка сообщения
				SendServer(msgs, 8 + s.Length);
			}
		}
	}
}
//для себя - лучше разделить по функциям, добавить тесты, переписать пару методов