using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Task9WF.Interfaces;

namespace Task9WF
{
    public class SocketManager : IConnectionManager
    {
        public event EventHandler<string> NewMessage;
        public event EventHandler<string> SystemMessage;
        public event EventHandler<string> ChangeConnection;

        TaskManager tasks = null;
        EndPoint localAddress = null;
        IMessager messager = null;
        string name = null;
        bool started = false;
        object constDataLocker = new object();
        public TaskManager TaskManager
        {
            set
            {
                if (tasks == null)
                    tasks = value;
            }
        }
        public EndPoint LocalAddress
        {
            set
            {
                lock (constDataLocker)
                {
                    if (localAddress == null)
                        localAddress = value;
                }
            }
        }
        public IMessager Messager
        {
            set
            {
                lock (constDataLocker)
                    if (messager == null)
                        messager = value;
            }
        }
        public string Name
        {
            get
            {
                lock (constDataLocker)
                {
                    return name == null ? "???" : name;
                }
            }
            set
            {
                lock (constDataLocker)
                {
                    if (name == null)
                    {
                        name = value;
                        tasks.Run(() => SendToAll(MessageType.Name, Name));
                    }
                }
            }
        }
        public bool Started
        {
            get
            {
                lock (constDataLocker)
                {
                    return started;
                }
            }
        }

        List<SocketHandler> socketHandlerList = new List<SocketHandler>();
        Hashtable connections = new Hashtable();
        bool stopped = false;
        List<EndPoint> waitingForConnection = new List<EndPoint>();
        object socketListLocker = new object();

        public void Start()
        {
            lock (constDataLocker)
            {
                if (!started)
                    if (localAddress != null && messager != null)
                    {
                        tasks.Run(() => ProcessingIncomingSockets());
                        lock (constDataLocker)
                        {
                            started = true;
                        }
                    }
            }
        }

        bool AddressKnown(EndPoint address)
        {
            return connections.ContainsKey(address) || connections.ContainsValue(address) || localAddress.Equals(address);
        }

        public void AddConnection(object sender, Connection connection)
        {
            while (true)
            {
                lock (socketListLocker)
                {
                    if (stopped)
                    {
                        connection.Socket.Close();
                        return;
                    }
                }
                lock (constDataLocker)
                {
                    if (started)
                        break;
                }
                Thread.Sleep(1000);
            }

            lock (socketListLocker)
            {
                if (stopped || AddressKnown(connection.RemoteEndPoint) || AddressKnown(connection.SocketListener))
                {
                    connection.Socket.Close();
                    return;
                }

                if (!messager.SendMessage(connection.Socket, MessageType.Socket, localAddress.ToString()))
                {
                    connection.Socket.Close();
                    return;
                }

                SendToAll(MessageType.Socket, connection.SocketListener.ToString());

                foreach (EndPoint endPoint in connections.Values)
                    if (!messager.SendMessage(connection.Socket, MessageType.Socket, endPoint.ToString()))
                    {
                        connection.Socket.Close();
                        return;
                    }

                if(!messager.SendMessage(connection.Socket, MessageType.Name, Name))
                {
                    connection.Socket.Close();
                    return;
                }

                connections[connection.RemoteEndPoint] = connection.SocketListener;

                SocketHandler socketHandler = new SocketHandler(
                    connection, 
                    SocketReceived, 
                    SocketStoppedError, 
                    NewMessage, 
                    ChangeConnection,
                    messager);

                tasks.Run(() => socketHandler.Run());

                socketHandlerList.Add(socketHandler);

                ChangeConnection(connection.SocketListener, socketHandler.Name);

                SystemMessage(this, connection.SocketListener.ToString() + ": Connected");
            }
        }

        void SocketStoppedError(object sender, Connection connection)
        {
            connection.Socket.Close();
            lock (socketListLocker)
            {
                if (stopped)
                    return;

                socketHandlerList.Remove((SocketHandler)sender);
                connections.Remove(connection.RemoteEndPoint);
            }
            Thread.Sleep(1000);
            string error = TryConnect(connection.SocketListener);
            if (error != null && error != "Already connected")
            {
                ChangeConnection(connection.SocketListener, null);
                SystemMessage(this, connection.SocketListener.ToString() + " " + ((SocketHandler)sender).Name + ": Disconnected");
            }
        }

        void SocketReceived(object sender, EndPoint address)
        {
            if (address == null)
                return;
            lock (socketListLocker)
            {
                if (AddressKnown(address) || waitingForConnection.Contains(address))
                    return;

                waitingForConnection.Add(address);
            }
        }

        void ProcessingIncomingSockets()
        {
            while (true)
            {
                while (true)
                {
                    EndPoint endPoint;

                    lock (socketListLocker)
                    {
                        if (stopped)
                            return;
                        if (waitingForConnection.Count <= 0)
                            break;

                        endPoint = waitingForConnection[0];
                        waitingForConnection.RemoveAt(0);
                    }

                    TryConnect(endPoint);
                }

                Thread.Sleep(1000);
            }
        }
        public string TryConnect(string address, int port)
        {
            try
            {
                return TryConnect(new IPEndPoint(IPAddress.Parse(address), port));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string TryConnect(EndPoint remoteEndPoint)
        {
            lock (socketListLocker)
            {
                if (stopped)
                    return null;
                if (AddressKnown(remoteEndPoint) || waitingForConnection.Contains(remoteEndPoint))
                    return "Already connected";
            }

            Socket socket = new Socket(IPAddress.Parse("127.0.0.1").AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(remoteEndPoint);
            }
            catch (Exception ex)
            {
                socket.Close();
                return ex.Message;
            }

            AddConnection(this, new Connection { Socket = socket, RemoteEndPoint = socket.RemoteEndPoint, SocketListener = remoteEndPoint });

            try
            {
                var b = socket.Blocking;
            }
            catch
            {
                return "Connection canceled";
            }

            return null;
        }

        public void SendToAll(MessageType type, string message)
        {
            lock (socketListLocker)
            {
                if (stopped)
                    return;
                for (int i = 0; i < socketHandlerList.Count; i++)
                    socketHandlerList[i].Send(type, message);
            }
        }

        public void Stop()
        {
            lock (constDataLocker)
            {
                started = false;
            }
            lock (socketListLocker)
            {
                if (stopped)
                    return;

                foreach (SocketHandler socketHandler in socketHandlerList)
                    socketHandler.Connection.Socket.Close();

                socketHandlerList = null;
                connections = null;
                waitingForConnection = null;
                stopped = true;
            }
        }
    }
}
