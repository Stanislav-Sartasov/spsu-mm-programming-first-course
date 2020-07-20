using Chat.StringMethod;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client
{
    public class User
    {
        List<IPEndPoint> listOfConnections;
        private Socket socket;
        private IPEndPoint yourIp;
        public string Name { get; private set; }
        public User()
        {
            listOfConnections = new List<IPEndPoint>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            const int SIO_UDP_CONNRESET = -1744830452;
            byte[] inValue = new byte[] { 0 };
            byte[] outValue = new byte[] { 0 };

            socket.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
        }
        public IPEndPoint ShowYourIp()
        {
            return yourIp;
        }
        public List<IPEndPoint> ShowListOfConnections()
        {
            List<IPEndPoint> temp = new List<IPEndPoint>(listOfConnections);
            return temp;
        }

        public bool GetAddress(string name, IPEndPoint address)
        {
            Name = name;
            try
            {
                socket.Bind(address);
            }
            catch
            {
                return false;
            }
            yourIp = address;
            Strings.WriteIp(yourIp);
            return true;
        }

        public void Connect(IPEndPoint interlocutorIp)
        {
            if (!Strings.CompareIp(yourIp, interlocutorIp) && !listOfConnections.Contains(interlocutorIp))
            {
                byte[] buffer = Strings.TranslateToByteArray(yourIp, listOfConnections, 1);
                socket.SendTo(buffer, interlocutorIp);
                listOfConnections.Add(interlocutorIp);
            }
        }

        public string GetMessage()
        {
            Console.WriteLine("*** Enter your message ***");
            return Console.ReadLine();
        }

        public void Disconnect()
        {
            if (listOfConnections.Count != 0)
            {
                byte[] buffer = Strings.TranslateToByteArray(yourIp, 2);
                foreach (IPEndPoint interlocutorIP in listOfConnections)
                    socket.SendTo(buffer, interlocutorIP);
                listOfConnections.Clear();
            }
        }

        public void ExitFromChat()
        {
            Disconnect();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Environment.Exit(0);
        }

        public void SendMessage(string message)
        {
            byte[] buffer = Strings.TranslateToByteArrayUnicode(message, 0);
            List<IPEndPoint> delList = new List<IPEndPoint>();
            foreach (IPEndPoint interlocutorIP in listOfConnections)
                    socket.SendTo(buffer, interlocutorIP);
        }

        public void Waiting()
        {
            Task waitingTask = new Task(Listen);
            waitingTask.Start();
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    int len = 0;
                    byte[] buffer = new byte[256];
                    List<byte> temp = new List<byte>();
                    EndPoint senderAdress = new IPEndPoint(IPAddress.Any, 0);
                    List<IPEndPoint> delList = new List<IPEndPoint>();

                    do
                    { 
                        len += socket.ReceiveFrom(buffer, ref senderAdress);
                        temp.AddRange(buffer);
                    }
                    while (socket.Available > 0);

                    foreach (var i in delList)
                        listOfConnections.Remove(i);

                    byte source = temp[0];
                    temp.RemoveRange(len, temp.Count - len);
                    if (source == 0)
                    {
                        string message = Strings.TranslateByteArrayToStringUnicode(temp.ToArray());
                        IPEndPoint senderIP = senderAdress as IPEndPoint;
                        Console.WriteLine("(from {0}:{1}) {2}", senderIP.Address.ToString(), senderIP.Port, message);
                    }
                    else
                        MessageHandler(temp);
                }
            }
            catch
            {
                Console.WriteLine("*** Check your connect ***");
                Listen();
            }
        }

        private void MessageHandler(List<byte> message)
        {
            byte source = message[0];
            message.RemoveAt(0);
            List<IPEndPoint> listOfIp = Strings.ParseByteListToAddress(message.ToArray());

            if (source == 2)
                DisconnectHandler(listOfIp);
            if (source == 1)
                ConnectHandler(listOfIp);
        }

        private void ConnectHandler(List<IPEndPoint> listOfIp)
        {
            
            bool isNewConnection = false;
            foreach (var ip in listOfIp)
                if (!listOfConnections.Contains(ip))
                {
                    listOfConnections.Add(ip);
                    isNewConnection = true;
                }
            if (isNewConnection)
            {
                Console.WriteLine("*** Now your interlocutors ***");
                List<IPEndPoint> interlocutors = ShowListOfConnections();
                foreach (var man in interlocutors)
                    Console.WriteLine($"{man}");

                foreach (IPEndPoint interlocutorIp in listOfConnections)
                {
                    List<IPEndPoint> currentIpList = ShowListOfConnections();
                    currentIpList.Remove(interlocutorIp);
                    byte[] buffer = Strings.TranslateToByteArray(yourIp, currentIpList, 1);
                    socket.SendTo(buffer, interlocutorIp);
                }
            }
        }

        private void DisconnectHandler(List<IPEndPoint> listOfIp)
        {
            listOfConnections.Remove(listOfIp[0]);
            Console.WriteLine($"*** {listOfIp[0]} disconnected ***");
        }
    }
}