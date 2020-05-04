using System;
using System.Net;
using System.Net.Sockets;


namespace P2PChatLibrary
{
    public class ClientUdp
    {
        internal const int maxUDPSize = 1024 * 4;
        internal int LocalPort { get; private set; }
        internal IPAddress LocalIP { get; private set; }
        internal Socket ListeningSocket { get; private set; }
        private Action<string> Output;

        public ClientUdp(int localPort, Action<string> output)
        {
            LocalPort = localPort;
            LocalIP = GetLocalIPAddress();
            ListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ListeningSocket.Bind(new IPEndPoint(GetLocalIPAddress(), localPort));
            Output = output;
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        internal void Send(byte[] dgram, IPEndPoint endPoint)
        {
            try
            {
                ListeningSocket.SendTo(dgram, endPoint);
            }
            catch (Exception e)
            {
                Output(e.Message);
            }
        }

        internal byte[] Receive(ref IPEndPoint remoteEP)
        {
            EndPoint tempRemoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data;
            data = new byte[maxUDPSize];
            int received = ListeningSocket.ReceiveFrom(data, maxUDPSize, 0, ref tempRemoteEP);
            remoteEP = (IPEndPoint)tempRemoteEP;
            if (received < maxUDPSize)
            {
                byte[] newBuffer = new byte[received];
                Buffer.BlockCopy(data, 0, newBuffer, 0, received);
                return newBuffer;
            }
            return data;
        }

        internal void Close()
        {
            if (ListeningSocket != null)
            {
                ListeningSocket.Shutdown(SocketShutdown.Both);
                ListeningSocket.Close();
                ListeningSocket = null;
            }
        }
    }
}
