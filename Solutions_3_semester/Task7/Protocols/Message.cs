using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace Protocols
{
	public class Message
	{
		public Message(SendingProtocols protocol, string content) : this((int)protocol, content) { }
		public Message(SendingProtocols protocol, byte[] content) : this((int)protocol, content) { }
		public Message(SendingProtocols protocol) : this((int)protocol) { }
		public Message(string content) : this(0, content) { }
		public Message(byte[] content) : this(0, content) { }
		public Message(int flag, string content)
		{
			Flag = flag;
			this.content = Encoding.ASCII.GetBytes(content);
		}
		public Message(int flag, byte[] content)
		{
			Flag = flag;
			this.content = content;
		}
		public Message(int flag)
		{
			Flag = flag;
		}

		byte[] content = new byte[1] { 0 };
		public string Content 
		{ 
			get
			{
				return Encoding.ASCII.GetString(content);
			}
		}
		public byte[] ContentByteArray
		{
			get
			{
				return content;
			}
		}
		public int Flag { get; }

		public static Message GetFromStream(TcpClient client, int timeout)
		{
			NetworkStream stream = client.GetStream();
			stream.ReadTimeout = timeout;
			byte[] lenBuffer = new byte[4];
			stream.Read(lenBuffer, 0, 4);
			int len = BitConverter.ToInt32(lenBuffer, 0);

			byte[] flag = new byte[4];
			byte[] message = null;
			try
			{
				message = new byte[len];
			}
			catch
			{
				Console.WriteLine(len);
			}
			stream.Read(flag, 0, 4);

			if (client.ReceiveBufferSize < len + 32)
			{
				client.SendBufferSize = len + 32;
				client.ReceiveBufferSize = len + 32;
			}
			stream = client.GetStream();
			stream.ReadTimeout = timeout;

			for (int i = 0; i < len; i++)
			{
				while (!stream.DataAvailable)
					Thread.Sleep(50);
				stream.Read(message, i, 1);
			}

			return new Message(BitConverter.ToInt32(flag, 0), message);
		}
		public void SendToStream(TcpClient client, int timeout)
		{
			byte[] flag = BitConverter.GetBytes(Flag);
			byte[] size = BitConverter.GetBytes(content.Length);
			byte[] allMessage = new byte[size.Length + content.Length + flag.Length];
			Array.Copy(size, allMessage, 4);
			Array.Copy(flag, 0, allMessage, 4, 4);
			Array.Copy(content, 0, allMessage, 8, content.Length);

			if (client.SendBufferSize < allMessage.Length + 32)
			{
				client.SendBufferSize = allMessage.Length + 32;
				client.ReceiveBufferSize = allMessage.Length + 32;
			}
			NetworkStream stream = client.GetStream();
			stream.WriteTimeout = timeout;
			stream.Write(allMessage, 0, allMessage.Length);
		}
	}
}
