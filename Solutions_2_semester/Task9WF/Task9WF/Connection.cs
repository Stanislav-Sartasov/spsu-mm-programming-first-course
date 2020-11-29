using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Task9WF
{
	public class Connection
	{
		Socket socket = null;
		Info info = null;
		string name = null;
		List<Message> messageBuffer = new List<Message>();
		object locker = new object();
		public bool Closed { get; private set; } = true;

		public Socket Socket
		{
			get
			{
				lock (locker)
				{
					return socket;
				}
			}
			set
			{
				lock (locker)
				{
					socket = value;
					Closed = false;
				}
			}
		}
		public Info Info
		{
			get
			{
				lock (locker)
				{
					return info;
				}
			}
			set
			{
				lock (locker)
				{
					info = value;
				}
			}
		}
		public string Name
		{
			get
			{
				lock (locker)
				{
					string addr = "";
					try
					{
						addr = Socket.RemoteEndPoint.ToString();
					}
					catch { }
					return addr + ": " + (name == null || name == "" ? "???" : name);
				}
			}
			set
			{
				lock (locker)
				{
					name = value;
				}

			}
		}
		public void AddMessage(Message message)
		{
			lock (locker)
			{
				messageBuffer.Add(message);
			}
		}
		public Message GetMessage()
		{
			lock (locker)
			{
				if (messageBuffer.Count == 0)
					return null;
				return messageBuffer[0];
			}
		}
		public void NextMessage()
		{
			lock (locker)
			{
				messageBuffer.RemoveAt(0);
			}
		}
		public bool Connected
		{
			get
			{
				try
				{
					lock (locker)
					{
						bool key = Socket.Connected;
						return key;
					}
				}
				catch
				{
					return false;
				}
			}
		}
		public void Close()
		{
			lock (locker)
			{
				try
				{
					if (!Closed)
						Socket.Close();
				}
				finally
				{
					Closed = true;
				}
			}
		}
	}
}
