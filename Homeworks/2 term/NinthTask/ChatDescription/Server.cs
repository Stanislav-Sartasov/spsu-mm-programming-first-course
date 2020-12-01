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
		Thread ThreadList;
		Thread ThreadRead;
		void StartListening() // Listening for new users
		{
			byte[] bytes;
			IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);

			ChatManager.Listener = new Socket(AddressFamily.InterNetwork,
			    SocketType.Stream, ProtocolType.Tcp);
			while (true)
			{
				try
				{
					ChatManager.Listener.Bind(localEndPoint);
				}
				catch (Exception e)
				{
					localEndPoint.Port += 1;
					continue;

				}
				break;
			}
			Console.WriteLine("Address Server: " + localEndPoint.ToString());
			try
			{
				ChatManager.Listener.Listen(100);

				while (ChatManager.FlagWorking)
				{
					Socket handler = ChatManager.Listener.Accept();
					bytes = new byte[1024];
					int bytesRec = handler.Receive(bytes);
					if (bytesRec == 4 && BitConverter.ToInt32(bytes, 0) == 0)
					{
						ChatManager.MutexSocketList.WaitOne();
						ChatManager.SocketList.Add(handler.RemoteEndPoint.ToString(), handler);
						Console.WriteLine("New User: " + handler.RemoteEndPoint.ToString());
						ChatManager.MutexSocketList.ReleaseMutex();
					}
					else
					{
						handler.Close();
					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

		}

		void GetRead() // Client data receiving
		{
			while (ChatManager.FlagWorking)
			{
				ChatManager.MutexSocketList.WaitOne();
				var iRemove = ChatManager.SocketList.Where(f => f.Value.Connected == false).ToArray();
				foreach (var i in iRemove)
				{
					ChatManager.SocketList.Remove(i.Key);
				}

				foreach (var i in ChatManager.SocketList)
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
							ChatManager.SendOthers(i.Key, data, 0);
						}
						if (code == 3)
						{
							int len = BitConverter.ToInt32(bdata, 4);
							string data = Encoding.ASCII.GetString(bdata, 8, len);
							Console.WriteLine(data);
							ChatManager.ForwardOther(bdata, 8 + len, i.Key);
						}
					}
				}
				ChatManager.MutexSocketList.ReleaseMutex();
				Thread.Sleep(0);
			}
		}
		public void Start()
		{
			ThreadList = new Thread(StartListening);
			ThreadList.Start();
			ThreadRead = new Thread(GetRead);
			ThreadRead.Start();
		}
		public void Join()
		{
			ThreadList.Join();
			ThreadRead.Join();
		}
		public void SendClient(byte[] msg, int size)
		{
			ChatManager.MutexSocketList.WaitOne();
			foreach (var i in ChatManager.SocketList)
			{
				try
				{
					i.Value.Send(msg, size, SocketFlags.None);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
			ChatManager.MutexSocketList.ReleaseMutex();
		}
	}
}
