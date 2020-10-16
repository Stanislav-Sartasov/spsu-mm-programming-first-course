using System.Net;
using System.Net.Sockets;

namespace Task9WF
{
    public struct Connection
    {
        public Socket Socket { get; set; }
        public EndPoint RemoteEndPoint { get; set; }
        public EndPoint SocketListener { get; set; }
    }
}
