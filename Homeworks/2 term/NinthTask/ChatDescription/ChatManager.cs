using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NinthTask.ChatDescription
{
	class ChatManager
	{
		//Generic information, used for both client and server
		public static bool FlagWorking; // флаг работы
		public static Dictionary<string, Socket> SocketList; // список пользователей (адрес/имя, сокет)
		public static Socket Listener;
		public static Socket SocketServer; // ServerAdress
		public static Mutex MutexSocketList; // мьютекс на доступ к словарю
		public static Mutex MutexSocketServer; // мьютекс на доступ к сокету сервера
		
		public static void SendOthers(string name, string message, int onlyUsers)
		{
			var bytes = new byte[1024];

			BitConverter.GetBytes(3).CopyTo(bytes, 0);
			string sMessage = name + "> " + message;
			BitConverter.GetBytes(sMessage.Length).CopyTo(bytes, 4);
			Encoding.ASCII.GetBytes(sMessage).CopyTo(bytes, 8);

			int sizeOfBytes = 8 + sMessage.Length;

			if (onlyUsers == 1)
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
			else
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
		}

		/*
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
		*/
		public static void ForwardOther(byte[] bytes, int sizeOfBytes, string name = null, int onlyUsers = 0)
		{
			if (onlyUsers == 1)
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
			else
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
		}

		/*
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
		*/

		public ChatManager()
		{
			FlagWorking = true;
			SocketList = new Dictionary<string, Socket>();
			Listener = null;
			SocketServer = null;
			MutexSocketList = new Mutex();
			MutexSocketServer = new Mutex();
		}

		public void Start(string[] args)
		{
			var server = new Server();
			server.Start();
			Client client = null;

			//Server connection
			if (args.Length > 0)
			{
				try
				{
					client = new Client();
					if (args.Length == 1)
					{
						client.Connect("127.0.0.1", int.Parse(args[0]));
					}
					else if (args.Length == 2)
					{
						client.Connect(args[0], int.Parse(args[1]));
					}

					client.Start();
				}
				catch (Exception e)
				{
					if (((SocketException)e).SocketErrorCode == SocketError.ConnectionRefused ||
					    ((SocketException)e).SocketErrorCode == SocketError.TimedOut)
					{
						Console.WriteLine("Connection error: " + (args.Length == 2 ? args[0] + ":" + args[1] : "127.0.0.1:" + args[0])); // переписать if
					}
					client = null;
				}
			}

			while (true)
			{
				//waiting
				string s = Console.ReadLine();

				if (s == "exit") // exit
				{
					//send exit
					Listener.Close();

					FlagWorking = false;

					if (server != null)
					{
						server.Join();
					}
					if (client != null)
					{
						client.Join();
					}
					break;
				}

				//coding string
				byte[] messages = new byte[1024];
				BitConverter.GetBytes(2).CopyTo(messages, 0);
				BitConverter.GetBytes(s.Length).CopyTo(messages, 4);
				Encoding.ASCII.GetBytes(s).CopyTo(messages, 8);
				//

				//sending
				//server.SendClient(messages, 8 + s.Length);
				MutexSocketList.WaitOne();
				foreach (var i in SocketList)
				{
					try
					{
						i.Value.Send(messages, 8 + s.Length, SocketFlags.None);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.ToString());
					}
				}
				MutexSocketList.ReleaseMutex();

				if (client != null)
				{
					//client.SendServer(messages, 8 + s.Length);
					MutexSocketServer.WaitOne(); // Sending message to server (if it exists) and all users
					if (SocketServer != null && SocketServer.Connected == true)
					{
						try
						{
							SocketServer.Send(messages, 8 + s.Length, SocketFlags.None);
						}
						catch (Exception e)
						{
							Console.WriteLine(e.Message);
						}
					}
					MutexSocketServer.ReleaseMutex();
				}
			}
		}
	}
}