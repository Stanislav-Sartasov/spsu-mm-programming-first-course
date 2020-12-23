using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NinthTask.ChatDescription
{
	public class ChatManager
	{
		//Generic information, used for both client and server
		public bool isServerAlive; // флаг работы
		public Dictionary<string, Socket> SockerUserList; // список пользователей (адрес/имя, сокет)
		private Socket ServerAddress;

		public Mutex MutexSocketList; // мьютекс на доступ к словарю
		public Mutex MutexSocketServer; // мьютекс на доступ к сокету сервера

		Thread ThreadReadServer;

		public void SendOthers(string name, string message, bool onlyUsers)
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

				this.MutexSocketServer.WaitOne();

				if (ServerAddress != null && ServerAddress.Connected)
				{
					ServerAddress.Send(bytes, sizeOfBytes, SocketFlags.None);
				}

				MutexSocketServer.ReleaseMutex();
			}
		}

		public void ForwardOther(byte[] bytes, int sizeOfBytes, string name = null, bool onlyUsers = false)
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
				MutexSocketServer.WaitOne();

				if (ServerAddress != null && ServerAddress.Connected == true)
				{
					ServerAddress.Send(bytes, sizeOfBytes, SocketFlags.None);
				}

				MutexSocketServer.ReleaseMutex();
			}
		}

		public ChatManager()
		{
			isServerAlive = true;
			SockerUserList = new Dictionary<string, Socket>();
			ServerAddress = null;
			// ServerAddress = null;
			MutexSocketList = new Mutex();
			MutexSocketServer = new Mutex();
		}

		void GetReadServer() // Server data monitoring
		{
			while (isServerAlive)
			{
				MutexSocketServer.WaitOne();

				if (ServerAddress.Available > 0)
				{
					byte[] bt = new byte[1024];
					int sz = ServerAddress.Receive(bt);
					int code = BitConverter.ToInt32(bt, 0);

					Console.WriteLine(code);

					if (code == 3)
					{
						int len = BitConverter.ToInt32(bt, 4);
						string data = Encoding.ASCII.GetString(bt, 8, len);
						Console.WriteLine(data);
						ForwardOther(bt, 8 + len, null, true);
					}
					if (code == 2)
					{
						int len = BitConverter.ToInt32(bt, 4);
						string data = Encoding.ASCII.GetString(bt, 8, len);
						Console.WriteLine("{0}> {1}", ServerAddress.RemoteEndPoint.ToString(), data);
						SendOthers(ServerAddress.RemoteEndPoint.ToString(), data, true);
					}
				}

				MutexSocketServer.ReleaseMutex();
				Thread.Sleep(0);
			}
		}

		void StartReadServer()
		{
			ThreadReadServer = new Thread(GetReadServer);
			ThreadReadServer.Start();
		}

		public void StartServer()
		{
			var server = new Server();
			server.Start();
		}

		private byte[] ConvertInputStringToByteArray(string input)
		{
			byte[] messages = new byte[1024];
			BitConverter.GetBytes(2).CopyTo(messages, 0);
			BitConverter.GetBytes(input.Length).CopyTo(messages, 4);
			Encoding.ASCII.GetBytes(input).CopyTo(messages, 8);
			return messages;
		}

		private void ConnectToServer(string port, string ip)
		{
			try
			{
				var client = new Client();
				ServerAddress = client.Connect(ip, int.Parse(port));
			}
			catch (Exception e)
			{
				if (((SocketException)e).SocketErrorCode == SocketError.ConnectionRefused ||
					((SocketException)e).SocketErrorCode == SocketError.TimedOut)
				{
					Console.WriteLine($"Connection error: 127.0.0.1:{port}"); // переписать if
				}
				else
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		public void StartChatting(string port, string ip = "127.0.0.1")
		{

			ConnectToServer(port, ip);

			try
			{
				StartReadServer();

				Console.WriteLine("if you want to exit type 'exit'");

				var inputStr = Console.ReadLine();

				while (true)
				{
					if (inputStr == "exit") // exit
					{
						break; // вылетает
					}
					else
					{
						var message = ConvertInputStringToByteArray(inputStr);

						MutexSocketList.WaitOne();
						foreach (var pair in SockerUserList)
						{
							try
							{
								var socket = pair.Value;
								socket.Send(message, 8 + inputStr.Length, SocketFlags.None);
							}
							catch (Exception e)
							{
								Console.WriteLine(e.ToString());
							}
						}
						MutexSocketList.ReleaseMutex();

						//client.SendServer(messages, 8 + s.Length);
						MutexSocketServer.WaitOne(); // Sending message to server (if it exists) and all users

						if (ServerAddress != null && ServerAddress.Connected == true)
						{
							try
							{
								ServerAddress.Send(message, 8 + inputStr.Length, SocketFlags.None);
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
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}