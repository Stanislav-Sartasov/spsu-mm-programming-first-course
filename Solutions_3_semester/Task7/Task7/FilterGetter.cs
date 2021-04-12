using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Protocols;

namespace Task7
{
	static class FilterGetter
	{
		public static string[] GetFilters(IPEndPoint endPoint)
		{
			TcpClient client = new TcpClient();
			client.Connect(endPoint);
			new Message(SendingProtocols.FilterSending).SendToStream(client, 5000);
			string[] filters = Message.GetFromStream(client, 5000).Content.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			client.Close();
			return filters;
		}
	}
}
