using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics.Contracts;

namespace P2PChat
{
    public class Engine
    {
        private bool stop, fail;
        private readonly IInteraction inter;
        private List<IPEndPoint> ipList;
        private IPEndPoint myIp;
        private Socket socketListen, socketPost;
        private Task<RecivedMessage> listenTask;
        private Task<string> enteredMessage;
        public Engine()
        {
            stop = false;
            fail = false;
            inter = new Interaction();
            ipList = new List<IPEndPoint>();
            inter.Show("print your port to recive messages\n");
            bool flag = false;
            socketListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            do 
            {
                flag = false;
                int port = inter.GetPort();
                string ipStr = inter.GetIp();
                myIp = new IPEndPoint(IPAddress.Parse(ipStr), port);
                try
                {
                    socketListen.Bind(myIp);
                }
                catch (Exception)
                {
                    flag = true;
                    inter.Show("Exeption, probably this port is not available. type another port\n");
                }
            } while (flag);
            socketPost = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketListen.Listen(10);
        }
        public Engine(IInteraction start)
        {
            stop = false;
            fail = false;
            inter = start;////////////////////////
            ipList = new List<IPEndPoint>();
            inter.Show("print your port to recive messages\n");
            bool flag = false;
            socketListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            do
            {
                flag = false;
                int port = inter.GetPort();
                myIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                try
                {
                    socketListen.Bind(myIp);
                }
                catch (Exception)
                {
                    flag = true;
                    inter.Show("Exeption, probably this port is not available. type another port\n");
                }
            } while (flag);
            socketPost = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketListen.Listen(10);
        }
        public void GetMessage()
        {
            if (listenTask.Status == TaskStatus.RanToCompletion)
            {
                RecivedMessage recivedMessage = listenTask.Result;
                if ((recivedMessage.message[recivedMessage.message.Length - 1] == '\n') || (recivedMessage.message[recivedMessage.message.Length - 1] == '\0'))
                {
                    string str = "";
                        for (int i = 0; i < recivedMessage.message.Length - 1; ++i)
                            if ((recivedMessage.message[i] != '\0') && (recivedMessage.message[i] != '\n'))    
                                str = str + recivedMessage.message[i];
                    recivedMessage.message = str;
                }
                inter.SystemShow($"recived message ({recivedMessage.ip}:{recivedMessage.port}) - '{recivedMessage.message}'");
                string[] parsedAnswer = recivedMessage.message.Split('/');
                if (parsedAnswer[0] == "1")
                {
                    SystemPacket(recivedMessage);
                }
                else
                {
                    inter.ShowSender(recivedMessage);
                    inter.ShowMessage(recivedMessage);
                }
                listenTask.Dispose();
                listenTask = new Task<RecivedMessage>(() => ListenPort());
                listenTask.Start();
            }

        }
        private RecivedMessage ListenPort()
        {
            Socket recivedSocket = socketListen.Accept();
            inter.SystemShow("accepted input");
            int bytes;
            byte[] data = new byte[256];
            string result = "";
            do
            {
                bytes = recivedSocket.Receive(data);
                result = result + Encoding.Unicode.GetString(data);
            }
            while (recivedSocket.Available > 0);
            RecivedMessage message = new RecivedMessage();
            message.message = result;
            message.ip = ((IPEndPoint)recivedSocket.RemoteEndPoint).Address.ToString();
            message.port = ((IPEndPoint)recivedSocket.RemoteEndPoint).Port;
            return message;
        }
        public void PostMessage()
        {
            if (enteredMessage.Status == TaskStatus.RanToCompletion)
            {
                string message = enteredMessage.Result;
                if (message == "exit()")
                {
                    stop = true;
                    return;
                }
                message = ((int)0).ToString() + "/" + myIp.Address.ToString() + "/" + myIp.Port.ToString() + "/" + message;
                byte[] data = Encoding.Unicode.GetBytes(message);
                for (int i = 0; i < ipList.Count; ++ i)
                {
                    IPEndPoint ip = ipList[i];
                    try
                    {
                        socketPost = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socketPost.Connect(ip);
                        socketPost.Send(data);
                        socketPost.Shutdown(SocketShutdown.Both);
                        socketPost.Close();
                        inter.SystemShow(1, $"good send {ip}");
                    }
                    catch (Exception e)
                    {
                        inter.SystemShow(1, $"bad try {ip}");
                        RecivedMessage re = new RecivedMessage();
                        re.message = ((int)1).ToString() + "/" + ((int)5).ToString() + "/" + ip.Address.ToString() + "/" + ip.Port.ToString();
                        re.ip = "127.0.0.0";
                        re.port = 0;
                        bool flag = SystemPacket(re);
                        if (!flag)
                        {
                            --i;
                        }
                    }
                }

                enteredMessage.Dispose();
                enteredMessage = new Task<string>(() => TypeMessage());
                enteredMessage.Start();
            }
        }
        public void WaitForConnect()
        {
            string sendString = ((int)1).ToString() + "/" + ((int)1).ToString() + "/" + myIp.Address.ToString() + "/" + myIp.Port.ToString();
            RecivedMessage recived = ListenPort();
            string[] parsedAnswer = recived.message.Split('/');
            if (parsedAnswer[0] == "1")
            {
                if ((recived.message[recived.message.Length - 1] == '\n') || (recived.message[recived.message.Length - 1] == '\0'))
                {
                    string str = "";
                    for (int i = 0; i < recived.message.Length - 1; ++i)
                        if ((recived.message[i] != '\0') && (recived.message[i] != '\n'))
                            str = str + recived.message[i];
                    recived.message = str;
                }
                SystemPacket(recived);
            }
            else
            {
                inter.SystemShow("What a f***? no system packet for connect");
            }
            listenTask = new Task<RecivedMessage>(() => ListenPort());
            listenTask.Start();
            enteredMessage = new Task<string>(() => TypeMessage());
            enteredMessage.Start();

        }
        public void OpenConnection(IPEndPoint sendIp)
        {
            listenTask = new Task<RecivedMessage>(() => ListenPort());
            listenTask.Start();

            string sendString = ((int)1).ToString() + "/" + ((int)1).ToString() + "/" + myIp.Address.ToString() + "/" + myIp.Port.ToString();
            if ((sendIp.Address.ToString() == myIp.Address.ToString()) && (sendIp.Port == myIp.Port))
            {
                stop = true;
                fail = true;
                inter.SystemShow(1, "you tried to connect to yourself....bad idea");
                enteredMessage = new Task<string>(() => TypeMessage());
                enteredMessage.Start();
                return;
            }
            ipList.Add(sendIp);
            Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sendSocket.Connect(sendIp);
                byte[] data = Encoding.Unicode.GetBytes(sendString);

                sendSocket.Send(data);

                sendSocket.Shutdown(SocketShutdown.Both);
                sendSocket.Close();

            }
            catch (Exception e)
            {
                inter.SystemShow(1, "bad ip for connect");
                stop = true;
                fail = true;
            }
            enteredMessage = new Task<string>(() => TypeMessage());
            enteredMessage.Start();
        }
        public bool CloseConnection()
        {
            if ((stop) && (!fail))
            {
                string message = ((int)1).ToString() + "/" + ((int)4).ToString() + "/" + myIp.Address.ToString() + "/" + myIp.Port.ToString();
                PostMessage(message);
                inter.Show("Disconnecting...\n");
            }
            if (fail)
            {
                inter.SystemShow(1, "fail system or fail start");
            }
            return stop || fail;
        }
        private bool SystemPacket(RecivedMessage recived)
        {
            string[] arrStr = recived.message.Split('/');
            if (arrStr[1] == "1")
            {
                 inter.SystemShow(1, $"we have got new man {arrStr[2]}:{arrStr[3]}, let us help him");
                string sendString = ((int)1).ToString() + "/" + ((int)3).ToString();
                foreach (var temp in ipList)
                {
                    sendString = sendString + "/" + temp.Address.ToString() + "/" + temp.Port.ToString();
                }
                string forTeam = ((int)1).ToString() + "/" + ((int)2).ToString() + "/" + arrStr[2] + "/" + arrStr[3];
                PostMessage(forTeam);
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(arrStr[2]), Int32.Parse(arrStr[3]));/////////////////////////////////////////////////
                socketPost = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socketPost.Connect(ip);
                    socketPost.Send(Encoding.Unicode.GetBytes(sendString));
                    socketPost.Shutdown(SocketShutdown.Both);
                    socketPost.Close();
                    ipList.Add(new IPEndPoint(IPAddress.Parse(arrStr[2]), Int32.Parse(arrStr[3])));
                }
                catch (Exception)
                {
                    inter.SystemShow(1, "bad help, can not send him ipList");
                }

            }
            if (arrStr[1] == "2")
            {
                 inter.SystemShow(1, $"new man here, info from {recived.ip}:{recived.port}, man is <{arrStr[2]}:{arrStr[3]}>");
                ipList.Add(new IPEndPoint(IPAddress.Parse(arrStr[2]), Int32.Parse(arrStr[3]))); ///
            }
            if (arrStr[1] == "3")
            {
                inter.SystemShow($"input list recived from {recived.ip}:{recived.port}");
                for (int i = 2; i < arrStr.Length; ++i)
                {
                    string ip = arrStr[i];
                    ++i;
                    string port = arrStr[i];
                    IPEndPoint temp = new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port));
                    ipList.Add(temp);
                }
            }
            if (arrStr[1] == "4")
            {
                 inter.SystemShow(1, $"ooh, man is gone {arrStr[2]}:{arrStr[3]}, everything is ok (packet 4)");
                IPEndPoint temp = new IPEndPoint(IPAddress.Parse(arrStr[2]), Int32.Parse(arrStr[3]));
                ipList.Remove(temp);
            }
            if (arrStr[1] == "5")
            {
                 inter.SystemShow(1, $"Hmm, man did not answer for us {arrStr[2]}:{arrStr[3]}");
                IPEndPoint temp = new IPEndPoint(IPAddress.Parse(arrStr[2]), Int32.Parse(arrStr[3]));
                int j = 0;
                for (int i = 0; i < 3; ++i)
                {
                    try
                    {
                        socketPost = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socketPost.Connect(temp);
                        //socketPost.Send(Encoding.Unicode.GetBytes(sendString));
                        socketPost.Shutdown(SocketShutdown.Both);
                        socketPost.Close();
                    }
                    catch (Exception)
                    {
                        ++j;
                           inter.SystemShow($"did not answer {j} times");
                    }
                }
                if (j == 3)
                {
                    inter.SystemShow(1, $"man {arrStr[2]}:{arrStr[3]} is gone with error");
                    int index = ipList.IndexOf(temp);
                    if (index >= 0)
                    {
                        ipList.RemoveAt(index);
                        return false;
                    }
                    else
                        inter.SystemShow("error delete from ipList");
                }
                else
                {
                    inter.SystemShow(1, "He is with us again, but without a message");
                    return true;
                }
            }
            return true;
        }
        public string TypeMessage()
        {
            return inter.GetStr();
        }
        public void PostMessage(string message)
        {
                byte[] data = Encoding.Unicode.GetBytes(message);
                for (int i = 0; i < ipList.Count; ++i)
                {
                    IPEndPoint ip = ipList[i];
                    try
                        {
                            socketPost = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            socketPost.Connect(ip);
                            socketPost.Send(data);
                            socketPost.Shutdown(SocketShutdown.Both);
                            socketPost.Close();
                            inter.SystemShow(1, $"good system send {ip}");
                        }
                        catch (Exception e)
                        {
                            inter.SystemShow(1, $"bad try system send{ip}");
                            RecivedMessage re = new RecivedMessage();
                            re.message = ((int)1).ToString() + "/" + ((int)5).ToString() + "/" + ip.Address.ToString() + "/" + ip.Port.ToString();
                            re.ip = "127.0.0.0";
                            re.port = 0;
                            SystemPacket(re);                    
                        }
                }
        }
    }
}
