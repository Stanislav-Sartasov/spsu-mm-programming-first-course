using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using P2P;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using P2P.Serialization;
using System.Linq;

namespace P2P.Clients
{
    public class Client
    {
        private static Socket socket;
        private static EndPoint local;
        private static List<EndPoint> clients;
        private enum Signals
        {
            Message = '0',
            Connect = '1',
            Updates = '2',
            Disconnect = '3'
        }
        public Client()
        {
            clients = new List<EndPoint>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        public void SetClient()
        {
            clients.Add(local);
        }
        public void SetLocalPoint(IPEndPoint endPoint)
        {
            local = endPoint;
        }
        public List<EndPoint> GetClients()
        {
            List<EndPoint> temp = new List<EndPoint>(clients);
            return temp;
        }
        public void Connect(EndPoint endPoint)
        {
            
            if (!clients.Contains(endPoint))
            {
                socket.SendTo(Serialization.Serialization.Serialize(clients, (char)Signals.Connect), endPoint);
                clients.Add(endPoint);
            }
            else
            {
                Console.WriteLine("You are already connected to {0}. Try -connect again.", endPoint);
            }
        }
        
        public void Send(string data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Insert(0, 0);
            stringBuilder.Append(data);
            byte[] byteData = Encoding.Unicode.GetBytes(stringBuilder.ToString());
            foreach (EndPoint user in clients)
            {
                if (user != GetLocalPoint())
                    socket.SendTo(byteData, user);
            }
        }
        public EndPoint GetLocalPoint()
        {
            return local;
        }
        public bool BindSocket()
        {
            try
            {
                socket.Bind(local);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void Receive()
        {
            while (true)
            {

                EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                StringBuilder stringBuilder = new StringBuilder();
                int bytes = 0;
                Signals flagConnector = Signals.Message;
                byte[] data = new byte[1024];
                try
                {
                    do
                    {
                        bytes = socket.ReceiveFrom(data, ref remote);
                        stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    Console.WriteLine("Probably client does not exist");
                    continue;
                }

                char signal = stringBuilder[0];
                if (signal != (char)Signals.Message)
                    flagConnector = AcceptSignal(stringBuilder);
                   
                if (clients.Count > 1 && flagConnector == Signals.Connect)
                {
                    SendUpdatesToAll();
                }
                else if (flagConnector == Signals.Message && clients.Contains(remote))
                {
                    char[] temp = new char[stringBuilder.Capacity];
                    stringBuilder.CopyTo(1, temp, stringBuilder.Length - 1);
                    Console.WriteLine("{0} : {1}", remote.ToString(), new string(temp));
                }
                
            }
        }
        public void SendUpdatesToAll()
        {
            foreach (EndPoint client in clients)
            {
                if (client != local)
                    socket.SendTo(Serialization.Serialization.Serialize(clients, (char)Signals.Updates), client);
            }
        }
        private static void UpdateClientList(StringBuilder stringBuilder, Signals signal)
        {
            List<EndPoint> users = Serialization.Serialization.Deserialize(stringBuilder);
            if (signal == Signals.Connect || signal == Signals.Updates)
            {

                foreach (EndPoint user in users)
                {
                        
                    if (!clients.Contains(user))
                    {
                        Console.WriteLine("User {0} connected to room\n", user);
                        clients.Add(user);
                    }
                }
                if (clients.Count > 1)
                {
                    Console.WriteLine("Current clients in room:");
                    foreach (EndPoint client in clients)
                    {
                        Console.WriteLine(client.ToString());
                    }
                    Console.WriteLine();
                }
            }
            else if (signal == Signals.Disconnect)
            {
                if (clients.Count > 1)
                {
                    Console.WriteLine("Client {0} leaves this room.", users[0]);
                    if (clients.Contains(users[0]))
                        clients.Remove(users[0]);
                }
            }
        }
        private static Signals AcceptSignal(StringBuilder stringBuilder)
        {
            foreach (Signals signal in Enum.GetValues(typeof(Signals)))
            {
                if (stringBuilder[0] == (char)signal)
                {
                    if (signal != Signals.Message)
                        UpdateClientList(stringBuilder, signal);
                    return signal;
                }
            }
            return Signals.Message;
        }
        public void Exit()
        {
            Disconnect();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Environment.Exit(0);
        }

        public void Disconnect()
        {
            if (clients.Count > 1)
            {
                List<EndPoint> temp = new List<EndPoint>(clients);
                clients = new List<EndPoint>();
                clients.Add(local);

                foreach (EndPoint client in temp)
                {
                    if (client != local)
                        socket.SendTo(Serialization.Serialization.Serialize(clients, (char)Signals.Disconnect), client);
                }
                Console.WriteLine("Disconnected from the room.");
            }
        }

        public void Listen()
        {
            Task listenTask = new Task(Receive);
            listenTask.Start();
        }
       
    }
}
