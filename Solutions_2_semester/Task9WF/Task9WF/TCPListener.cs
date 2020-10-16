using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Task9WF.Interfaces;

namespace Task9WF
{
    public class TCPListener : IListener
    {
        bool inited = false;
        public bool Started { get; private set; } = false;
        public event EventHandler<Connection> NewConnection;
        TaskManager tasks;
        IPEndPoint ipEndPoint;
        Socket socket;
        object locker = new object();
        IMessager messager;
        public IMessager Messager
        {
            set
            {
                lock (locker)
                {
                    if (messager == null)
                        messager = value;
                }
            }
        }

        public EndPoint LocalAddress
        {
            get
            {
                return ipEndPoint;
            }
        }

        public TaskManager TaskManager
        {
            set
            {
                lock (locker)
                {
                    if (tasks == null)
                        tasks = value;
                }
            }
        }

        public bool Init()
        {
            return Init(49152, 65535);
        }
        public bool Init(int startPort)
        {
            if (startPort < 0)
                return Init();
            else
                return Init(new int[] { startPort });
        }
        public bool Init(int portMin, int portMax)
        {
            lock (locker)
            {
                if (Started)
                    return false;
            }

            List<int> portsList = new List<int> { portMin };
            for (int i = portMin + 1; i < portMax; i++)     //like in Random.Next()
                portsList.Add(i);

            return Init(portsList.ToArray());
        }
        public bool Init(int[] portsList)
        {
            lock (locker)
            {
                if (Started)
                    return false;

                IPAddress address = IPAddress.Parse("127.0.0.1");
                socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                foreach (int i in portsList)
                {
                    try
                    {
                        ipEndPoint = new IPEndPoint(address, i);
                        socket.Bind(ipEndPoint);
                        break;
                    }
                    catch
                    {
                        ipEndPoint = null;
                    }
                }

                if (ipEndPoint == null || messager == null || tasks == null)
                {
                    inited = false;
                    return false;
                }

                inited = true;
                return true;
            }
        }
        public async void Start(int backlog)
        {
            lock (locker)
            {
                if (!inited)
                    return;

                Started = true;
                socket.Listen(backlog);
            }

            await tasks.Run(() =>
            {
                while (true)
                    try
                    {
                        Socket newSocket = socket.Accept();
                        WaiteForListener(newSocket);
                    }
                    catch
                    {
                        if (socket.Blocking)
                            return;
                    }
            });
        }

        async void WaiteForListener(Socket socket)
        {
            await tasks.Run(() =>
            {
                EndPoint address = messager.ReceiveMassege(socket).GetAddress();
                if (address == null)
                {
                    socket.Close();
                    return;
                }

                NewConnection(this, new Connection { Socket = socket, RemoteEndPoint = socket.RemoteEndPoint, SocketListener = address });
            });
        }
        
        public void Stop()
        {
            socket.Close();
            Started = false;
        }
    }
}