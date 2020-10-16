using System;
using System.Net;
using Task9WF.Interfaces;

namespace Task9WF
{
    class SocketHandler
    {
        public SocketHandler(
            Connection connection, 
            EventHandler<EndPoint> SocketAdding, 
            EventHandler<Connection> socketStoppedSubscriber, 
            EventHandler<string> newMessageSubscriber, 
            EventHandler<string> changeConnection,
            IMessager messager)
        {
            NewMessage += newMessageSubscriber;
            SocketRecived += SocketAdding;
            SocketStopped += socketStoppedSubscriber;
            ChangeConnection += changeConnection;
            this.messager = messager;
            Connection = connection;
        }

        string name = null;
        bool started = false;
        object constDataLocker = new object();
        public string Name
        {
            get
            {
                lock (constDataLocker)
                {
                    return name == null ? "???" : name;
                }
            }
            private set
            {
                lock (constDataLocker)
                {
                    name = value;
                }
            }
        }

        public Connection Connection { get; private set; }

        EventHandler<string> NewMessage;
        EventHandler<EndPoint> SocketRecived;
        EventHandler<Connection> SocketStopped;
        EventHandler<string> ChangeConnection;

        IMessager messager;

        public void Run()
        {
            lock (constDataLocker)
            {
                if (!started)
                    started = true;
            }
            Listen();
        }
        void Listen()
        {
            Message message;
            while (true)
            {
                message = messager.ReceiveMassege(Connection.Socket);
                if (message.Text == null)
                {
                    SocketStopped(this, Connection);
                    return;
                }

                switch (message.Type)
                {
                    case MessageType.Name:
                        Name = message.Text;
                        ChangeConnection(Connection.SocketListener, Name);
                        break;
                    case MessageType.Message:
                        NewMessage(Connection.SocketListener, message.Text);
                        break;
                    case MessageType.Socket:
                        SocketRecived(this, message.GetAddress());
                        break;
                }
            }
        }

        public void Send(MessageType type, string message)
        {
            if (!messager.SendMessage(Connection.Socket, type, message))
                SocketStopped(this, Connection);
        }
    }
}
