using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.IO;

using UserInterface;
using UserLibrary;
using System.Linq;

namespace Users
{
    public class User : IUser, ICommand, IMessage
    {
        private List<EndPoint> connectedUsers = new List<EndPoint>();
        public string Name { get; private set; }

        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        public EndPoint myEndPoint;

        public void Start()
        {
            socket.IOControl(-1744830452, new byte[] { 0 }, new byte[] { 0 });

            Thread clientThread = new Thread(new ThreadStart(StartClientPart));
            Initialize(1);

            clientThread.Start();
            StartServerPart();
        }

        public void StartClientPart()
        {
            while (true)
            {
                string message = Console.ReadLine();
                StringHandler.InputHandler(this, message);
            }
        }
        public void StartServerPart()
        {
            while (true)
            {
                EndPoint generalEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = new byte[1024];

                do
                {
                    int amountOfValues = 0;
                    List<byte> accumulateBuffer = new List<byte>();
                    amountOfValues += socket.ReceiveFrom(buffer, ref generalEndPoint);
                    accumulateBuffer.AddRange(buffer);
                    accumulateBuffer.RemoveRange(amountOfValues, accumulateBuffer.Count - amountOfValues);
                    StringHandler.MessageHandler(this, accumulateBuffer.ToArray());
                }
                while (socket.Available > 0);
            }
        }

        private void Initialize(int port)
        {
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    myEndPoint = new IPEndPoint(ip, port);

            try
            {
                socket.Bind(myEndPoint);
            }
            catch
            {
                Random randomPort = new Random();
                Initialize(randomPort.Next(1, 1 << 16));
                return;
            }
            Console.WriteLine("Your IP: " + myEndPoint.ToString());
            Console.WriteLine("Enter Nickname:");
            Name = Console.ReadLine();
        }

        public void SendMessage(string message)
        {
            byte[] buffer = ArrayMethods.ConcatArrays(new byte[] { (byte)Protocol.Message }, ArrayMethods.StringToArrayOfByte(Name + ": " + message));
            foreach (EndPoint user in connectedUsers)
                socket.SendTo(buffer, user);
        }

        public List<EndPoint> GetListOfConnectedUsers()
        {
            return connectedUsers;
        }

        public void Connect(EndPoint ipForConnection)
        {
            if (ipForConnection != myEndPoint)
            {
                Disconnect();
                byte[] buffer = ArrayMethods.ConcatArrays(new byte[] { (byte)Protocol.Connect }, ArrayMethods.EndPointToByteArray(myEndPoint));
                socket.SendTo(buffer, ipForConnection);
            }
            else
                Console.WriteLine("You cannot connect to yourself.");
        }

        public void Disconnect()
        { 
            byte[] buffer = ArrayMethods.ConcatArrays(new byte[] { (byte)Protocol.Disconnect }, ArrayMethods.EndPointToByteArray(myEndPoint));
            foreach (EndPoint user in connectedUsers)
                socket.SendTo(buffer, user);
            connectedUsers.Clear();
        }

        public void Exit()
        {
            Disconnect();
            Environment.Exit(0);
        }

        public void ConnectMessage(EndPoint connectedUserEndPoint)
        {
            if (!connectedUsers.Contains(connectedUserEndPoint))
            {
                byte[] buffer = ArrayMethods.ConcatArrays(new byte[] { (byte)Protocol.Connect }, ArrayMethods.EndPointToByteArray(connectedUserEndPoint));
                foreach (EndPoint user in connectedUsers)
                {
                    socket.SendTo(buffer, user);
                }
                List<EndPoint> connectedUsersToSend = new List<EndPoint>(connectedUsers)
                {
                    myEndPoint
                };
                byte[] answerBuffer = ArrayMethods.ConcatArrays(new byte[] { (byte)Protocol.ConnectAnswer }, ArrayMethods.ListEndPointToByteArray(connectedUsersToSend));
                socket.SendTo(answerBuffer, connectedUserEndPoint);

                connectedUsers.Add(connectedUserEndPoint);
            }
        }

        public void ConnectAnswerMessage(List<EndPoint> connectedUsers)
        {
            this.connectedUsers = connectedUsers;
        }

        public void DisconnectMessage(EndPoint disconnectedUserEndPoint)
        {
            connectedUsers.Remove(disconnectedUserEndPoint);
        }

        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
