using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Clients
{
    public class Client
    {
        private List<IPEndPoint> listOfInterlocutors;
        private Socket socket;
        private IPEndPoint yourIp;
        public string Name { get; private set; }
        public Client()
        {
            listOfInterlocutors = new List<IPEndPoint>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public IPEndPoint ViewYourIp()
        {
            return yourIp;
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
            Console.WriteLine($"*** Your IP in network: {yourIp.Address}:{yourIp.Port} ***");
            return true;
        }

        public void Connect(IPEndPoint interlocutorIp)
        {
            if (interlocutorIp.Equals(yourIp))
                Console.WriteLine("*** This is your IP. ***");
            else if (!listOfInterlocutors.Contains(interlocutorIp))
            {
                StringBuilder answer = new StringBuilder($"+++ The addresses {yourIp.Address}:{yourIp.Port},");
                foreach (IPEndPoint interlocutor in listOfInterlocutors)
                    answer.Append($"{interlocutor.Address}:{interlocutor.Port},");
                answer.Remove(answer.Length - 1, 1);
                answer.Append(" is connected to you. +++");
                byte[] buffer = Encoding.Unicode.GetBytes(answer.ToString());
                socket.SendTo(buffer, interlocutorIp);
                listOfInterlocutors.Add(interlocutorIp);
            }
        }

        public void Disconnect()
        {
            if (listOfInterlocutors.Count != 0)
            {
                SendMessageToAll($"--- The address {yourIp.Address}:{yourIp.Port} is disconnected from you. ---");
                listOfInterlocutors.Clear();
            }
        }

        //public void Disconnect()
        //{
        //    Console.WriteLine("*** Enter disconnect type. 1 - from all, other number - from a specific address ***");
        //    int type;
        //    while (true)
        //        try
        //        {
        //            string input = Console.ReadLine();
        //            if (input.Equals("stop"))
        //                return;
        //            type = int.Parse(input);
        //            break;
        //        }
        //        catch
        //        {
        //            Console.WriteLine("*** Something went wrong. Enter 1 to disconnect from all, other number from a specific address" +
        //                " If you don't want to disconnect, write 'stop' ***");
        //        }
        //    switch (type)
        //    {
        //        case 1:
        //            {
        //                SendMessageToAll($"--- The address {yourIp.Address}:{yourIp.Port} is disconnected from you. ---");
        //                ListOfInterlocutors.Clear();
        //                break;
        //            }
        //        default:
        //            {
        //                IPEndPoint interlocutorIp;
        //                while (true)
        //                {
        //                    Console.WriteLine("*** Write the address (IP:port) from which you want to disconnect. ***");
        //                    string inputAddress = Console.ReadLine();
        //                    if (inputAddress.Equals("stop"))
        //                        return;
        //                    if (IsCorrectInputAddress(inputAddress))
        //                    {
        //                        int borderOfIp = inputAddress.IndexOf(':');
        //                        interlocutorIp = new IPEndPoint(IPAddress.Parse(inputAddress.Substring(0, borderOfIp)),
        //                                                            int.Parse(inputAddress.Substring(borderOfIp + 1)));
        //                        break;
        //                    }
        //                    else
        //                        Console.WriteLine("*** Something went wrong, the address looks like x.x.x.x:y; 0 <= x <= 255, 1 <= y <= 65535." +
        //                            " If you don't want to disconnect, write 'stop' ***");
        //                }
        //                if (interlocutorIp.Equals(yourIp))
        //                    Console.WriteLine("*** This is your IP. ***");
        //                else if (ListOfInterlocutors.Contains(interlocutorIp))
        //                {
        //                    string answer = $"--- The address {yourIp.Address}:{yourIp.Port} is disconnected from you. ---";
        //                    byte[] buffer = Encoding.Unicode.GetBytes(answer);
        //                    socket.SendTo(buffer, interlocutorIp);
        //                    ListOfInterlocutors.Remove(interlocutorIp);
        //                }
        //                break;
        //            }
        //    }
        //}

        public List<IPEndPoint> ShowInterlocutors()
        {
            return listOfInterlocutors;
        }

        public void ExitFromChat()
        {
            Disconnect();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Environment.Exit(0);
        }


        public void SendMessageToAll(string message)
        {
            if (!(message.Substring(0, 3).Equals("---") || message.Substring(0, 3).Equals("+++")))
                message = Name + " : " + message;
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            foreach (IPEndPoint interlocutorIP in listOfInterlocutors)
                socket.SendTo(buffer, interlocutorIP);
        }

        public string GetMessage()
        {
            Console.WriteLine("*** Enter your message ***");
            return Console.ReadLine();
        }

        public static bool IsCorrectInputAddress(string address)
        {
            int i;
            for (i = 0; i < address.Length; i++)
                if ((address[i] < '0' || '9' < address[i]) && address[i] != '.' && address[i] != ':')
                    return false;

            i = 0;
            for (int j = 0; j < 4; j++)
            {
                StringBuilder ip = new StringBuilder("");
                while (address[i] != '.' && address[i] != ':')
                {
                    ip.Append(address[i]);
                    i++;
                }
                i++;
                if (int.Parse(ip.ToString()) < 0 || 255 < int.Parse(ip.ToString()))
                    return false;
            }

            string port = address.Substring(i);
            if (int.Parse(port.ToString()) < 0 || 65535 < int.Parse(port.ToString()))
                return false;

            return true;
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
                    StringBuilder message = new StringBuilder();
                    int bytes = 0;
                    byte[] buffer = new byte[256];

                    EndPoint senderAdress = new IPEndPoint(IPAddress.Any, 0);

                    do
                    {
                        bytes = socket.ReceiveFrom(buffer, ref senderAdress);
                        message.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                    }
                    while (socket.Available > 0);
                    IPEndPoint senderIP = senderAdress as IPEndPoint;
                    if (message[0] != '+' && message[0] != '-')
                        Console.WriteLine("(from {0}:{1}) {2}", senderIP.Address.ToString(), senderIP.Port, message);
                    else
                    {
                        MessageHandler(message.ToString());
                        if (message[3] != '!')
                            Console.WriteLine("(from {0}:{1}) {2}", senderIP.Address.ToString(), senderIP.Port, message);
                    }
                }
            }
            catch
            {
                Console.WriteLine("*** Check your connect ***");
            }
        }

        private void MessageHandler(string message)
        {
            string typeOfOperation = message.Substring(0, 3);
            int firstIndex;
            int secondIndex = message.IndexOf('i') - 2;
            for (firstIndex = 0; firstIndex < message.Length; firstIndex++)
                if ('0' <= message[firstIndex] && message[firstIndex] <= '9')
                    break;

            string mainPart = message.Substring(firstIndex, secondIndex - firstIndex + 1);

            List<IPEndPoint> listOfIp = new List<IPEndPoint>();
            List<string> stringAddresses = new List<string>(mainPart.Split(','));
            foreach(string address in stringAddresses)
            {
                int ipAddress = address.LastIndexOf(':');
                listOfIp.Add(new IPEndPoint(IPAddress.Parse(address.Substring(0, ipAddress)), int.Parse(address.Substring(ipAddress + 1))));
            }
            switch (typeOfOperation)
            {
                case "---":
                    {
                        DisconnectHandler(listOfIp);
                        break;
                    }
                case "+++":
                    {
                        ConnectHandler(listOfIp);
                        break;
                    }
            }
        }

        private void ConnectHandler(List<IPEndPoint> listOfIp)
        {
            bool isNewConnection = false;
            foreach (var ip in listOfIp)
                if (!listOfInterlocutors.Contains(ip))
                {
                    listOfInterlocutors.Add(ip);
                    isNewConnection = true;
                }
            if (isNewConnection)
            {
                Console.WriteLine("*** Now your interlocutors ***");
                List<IPEndPoint> interlocutors = ShowInterlocutors();
                foreach (var man in interlocutors)
                    Console.WriteLine($"{man.Address}:{man.Port}");

                foreach (IPEndPoint interlocutorIp in listOfInterlocutors)
                {
                    List<IPEndPoint> currentIpList = new List<IPEndPoint>();
                    currentIpList.AddRange(listOfInterlocutors);
                    currentIpList.Remove(interlocutorIp);
                    currentIpList.Add(yourIp);
                    StringBuilder messageWithAddresses = new StringBuilder($"+++! The addresses ");
                    foreach (IPEndPoint ip in currentIpList)
                        messageWithAddresses.Append($"{ip.Address}:{ip.Port},");
                    messageWithAddresses = messageWithAddresses.Remove(messageWithAddresses.Length - 1, 1);
                    messageWithAddresses.Append(" is connected to you. !+++");
                    byte[] buffer = Encoding.Unicode.GetBytes(messageWithAddresses.ToString());
                    socket.SendTo(buffer, interlocutorIp);
                }
            }    
        }

        private void DisconnectHandler(List<IPEndPoint> listOfIp)
        {
            listOfInterlocutors.Remove(listOfIp[0]);
            Console.WriteLine($"*** {listOfIp[0].Address}:{listOfIp[0].Port} disconnected ***");
        }
    }
}