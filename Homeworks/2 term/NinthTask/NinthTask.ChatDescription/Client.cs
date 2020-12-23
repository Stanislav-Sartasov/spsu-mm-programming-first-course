using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NinthTask.ChatDescription
{
	class Client
	{
		public Socket Connect(string address, int port)
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
			socket.Connect(ipPoint);

			Console.WriteLine("Address Local: " + socket.LocalEndPoint.ToString());
			socket.Send(BitConverter.GetBytes(0));

			return socket;
		}

	}
}