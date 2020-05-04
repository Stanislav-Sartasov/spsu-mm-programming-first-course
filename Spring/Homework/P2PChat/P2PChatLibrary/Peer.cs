using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace P2PChatLibrary
{
    public class Peer
    {
        public UserInfo LocalUser { get; private set; }
        private List<UserInfo> connectedUsers;
        private ClientUdp client;
        private Action<string> Output;
        public Peer(int localPort, string name, Action<string> output)
        {
            Output = output;
            client = new ClientUdp(localPort, Output);
            connectedUsers = new List<UserInfo>();
            LocalUser = new UserInfo(name, client.LocalIP.ToString(), localPort);
            connectedUsers.Add(LocalUser);
        }

        internal void SendAll(byte[] dgram)
        {
            if (connectedUsers != null)
            {
                foreach (var user in connectedUsers)
                {
                    if (!user.Equals(LocalUser))
                    {
                        client.Send(dgram, new IPEndPoint(IPAddress.Parse(user.Address), user.Port));
                    }
                }
            }
        }

        internal void SendDisconnectionInfo()
        {
            var messageInfo = new MessageInfo()
            {
                MesType = MessageType.Disconnection,
                Message = JsonConvert.SerializeObject(LocalUser)
            };
            var serialised = JsonConvert.SerializeObject(messageInfo);
            SendAll(Encoding.Unicode.GetBytes(serialised));
        }

        internal void SendConnectedUsersInfo(IPEndPoint endPoint, MessageType type)
        {
            var messageInfo = new MessageInfo()
            {
                MesType = type,
                Message = JsonConvert.SerializeObject(connectedUsers)
            };
            var serialised = JsonConvert.SerializeObject(messageInfo, Formatting.None);
            client.Send(Encoding.Unicode.GetBytes(serialised), endPoint);
        }

        public List<UserInfo> GetConnectedUsers() => connectedUsers;

        internal void PrintUsers()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Output("\nList of connected users:\n");
            foreach (var user in connectedUsers)
                Output($"{user.Name}({user.Address}:{user.Port})");
            Console.ResetColor();
        }

        internal void StartListening()
        {
            try
            {
                Task listeningTask = new Task(Listen);
                listeningTask.Start();
            }
            catch (Exception ex)
            {
                Output(ex.Message);
            }
        }

        internal void Listen()
        {
            while (true)
            {
                try
                {
                    StringBuilder builder = new StringBuilder();
                    byte[] data;
                    IPEndPoint remoteIp = null;
                    do
                    {
                        data = client.Receive(ref remoteIp);
                        builder.Append(Encoding.Unicode.GetString(data, 0, data.Length));
                    }
                    while (client.ListeningSocket.Available > 0);

                    var deserialized = JsonConvert.DeserializeObject<MessageInfo>(builder.ToString());
                    var messageType = deserialized.MesType;

                    if (messageType.Equals(MessageType.Message))
                    {
                        var message = JsonConvert.DeserializeObject<Message>(Convert.ToString(deserialized.Message));
                        Output($"{message.SenderName}" +
                            $": {message.MessageData}");
                    }
                    else if (messageType.Equals(MessageType.Disconnection))
                    {
                        UserInfo user = JsonConvert.DeserializeObject<UserInfo>(deserialized.Message);
                        if (connectedUsers.Contains(user))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Output($"{user.Name} leaves us!");
                            DeleteUser(user);
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(deserialized.Message);
                        if (messageType.Equals(MessageType.ConnectionRequest))
                        {
                            deserialized.MesType = MessageType.UsersAdding;
                            var serialised = JsonConvert.SerializeObject(deserialized);
                            SendAll(Encoding.Unicode.GetBytes(serialised));
                            foreach (var user in users)
                            {
                                SendConnectedUsersInfo(new IPEndPoint(IPAddress.Parse(user.Address), user.Port), MessageType.UsersAdding);
                            }
                        }
                        foreach (var user in users)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            if (!connectedUsers.Contains(user))
                            {
                                Output($"{user.Name} is now with us!");
                                connectedUsers.Add(user);
                            }
                            else
                            {
                                connectedUsers.Remove(user); //change from <unknown> to nickname
                                connectedUsers.Add(user);
                            }
                            Console.ResetColor();
                        }
                    }
                }
                catch
                {
                    connectedUsers.RemoveAll(u => u.Name.Equals("<unknown>"));
                }
            }
        }

        internal void DeleteUser(UserInfo user)
        {
            if (connectedUsers.Contains(user))
                connectedUsers.Remove(user);
        }

        internal void Disconnect()
        {
            SendDisconnectionInfo();
            connectedUsers.Clear();
            connectedUsers.Add(LocalUser);
        }

        internal void Connect(UserInfo user)
        {
            try
            {
                if (!LocalUser.Equals(user))
                {
                    SendConnectedUsersInfo(new IPEndPoint(IPAddress.Parse(user.Address), user.Port),
                        MessageType.ConnectionRequest);
                    connectedUsers.Add(user);
                }
            }
            catch(Exception ex)
            {
                Output(ex.Message);
            }
        }

        internal void Exit()
        {
            Disconnect();
            client.Close();
        }
    }
}
