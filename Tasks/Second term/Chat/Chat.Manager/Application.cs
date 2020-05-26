using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Chat.Clients;

namespace Chat.Manager
{
    public class Application
    {
        public void StartChating(Client client)
        {
            Console.WriteLine("Start");
            Console.WriteLine(Help());

            Console.WriteLine("*** Enter your name ***");
            string name = Console.ReadLine();
            int port = SelectionUserPort();
            while (!client.GetAddress(name, SelectionUserIpByPort(port)))
            {
                Console.WriteLine("*** Error. Try other port. ***");
                port = SelectionUserPort();
            }



            client.Waiting();
            while (true)
            {
                string message = client.GetMessage();
                if (message == null)
                    continue;

                if (message[0] == '/')
                {
                    message = message.ToLower();
                    bool isCommand = false;
                    if (message[1] == 'c')
                    {
                        if (message.Substring(1, 7).Equals("connect"))
                        {
                            string IpForConnection = message.Substring(9);
                            if (Client.IsCorrectInputAddress(IpForConnection))
                            {
                                int borderOfIP = IpForConnection.IndexOf(':');
                                client.Connect(new IPEndPoint(IPAddress.Parse(IpForConnection.Substring(0, borderOfIP)),
                                                                int.Parse(IpForConnection.Substring(borderOfIP + 1))));
                                isCommand = true;
                            }
                        }
                    }
                    else if (message[1] == 'd')
                    {
                        if (message.Substring(1).Equals("disconnect"))
                        {
                            client.Disconnect();
                            isCommand = true;
                        }
                    }
                    else if (message[1] == 'i')
                    {
                        if (message.Substring(1).Equals("interlocutors"))
                        {
                            List<IPEndPoint> interlocutors = client.ShowInterlocutors();
                            Console.WriteLine("*** Your connections ***");
                            foreach (var man in interlocutors)
                                Console.WriteLine($"{man.Address}:{man.Port}");
                            isCommand = true;
                        }
                    }
                    else if (message[1] == 'e')
                    {
                        if (message.Substring(1).Equals("exit"))
                        {
                            client.ExitFromChat();
                            isCommand = true;
                        }
                    }
                    else if (message[1] == 'h')
                    {
                        if (message.Substring(1).Equals("help"))
                        {
                            Console.WriteLine(Help());
                            isCommand = true;
                        }
                    }
                    if (!isCommand)
                        Console.WriteLine("*** Something went wrong. Uncorrect comand ***");
                }
                else
                    client.SendMessageToAll(message);
            }
        }

        private int SelectionUserPort()
        {
            int result = 0;
            while (true)
            {
                try
                {
                    Console.WriteLine("*** Input your port ***");
                    string input = Console.ReadLine();
                    result = int.Parse(input);
                    if (1 <= result && result <= 65535)
                        break;
                    else
                        throw new Exception();
                }
                catch (Exception)
                {
                    Console.WriteLine("*** Invalid port value, try not to enter extra characters. Just a number from 1 to 65535. ***");
                }
            }
            return result;
        }

        private IPEndPoint SelectionUserIpByPort(int port)
        {
            IPEndPoint ipEndPoint = null;
            IPAddress[] ipAddressesList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress ip in ipAddressesList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipEndPoint = new IPEndPoint(ip, port);
                    break;
                }
            }
            return ipEndPoint;
        }

        private string Help()
        {
            return "***\n Available commands: \n" +
                    "\n/Connect IP -  to connect to other people. IP looks like X.X.X.X:Y, where 0 <= x <= 255 and 1 <= y <= 65535.\n" +
                    "/Disconnect  -  to disconnect from all interlocutors\n" +
                    "/Interlocutors  -  to show your connections\n" +
                    "/Help - to show this information\n" +
                    "/Exit  -  to exit from chat\n***\n";
        }
    }
}