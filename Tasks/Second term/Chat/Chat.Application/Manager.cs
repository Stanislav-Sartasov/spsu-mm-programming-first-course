using Chat.Client;
using Chat.StringMethod;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Chat.Application
{
    public class Manager
    {
        private string Help()
        {
            return "***\n Available commands: \n" +
                    "\n/Connect IP -  to connect to other people. IP looks like X.X.X.X:Y, where 0 <= x <= 255 and 1 <= y <= 65535.\n" +
                    "/Disconnect  -  to disconnect from all interlocutors\n" +
                    "/Interlocutors  -  to show your connections\n" +
                    "/Help - to show this information\n" +
                    "/Exit  -  to exit from chat\n***\n";
        }

        public void StartChating(User client)
        {
            Console.WriteLine("Start");
            Console.WriteLine(Help());

            Console.WriteLine("*** Enter your name ***");
            string name = Console.ReadLine();
            int port = SelectionPort();

            while (!client.GetAddress(name, SelectionIpByPort(port)))
            {
                Console.WriteLine("*** Error. Try other port. ***");
                port = SelectionPort();
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
                    if (message[1] == 'c' && message.Length > 9)
                    {
                        if (message.Substring(1, 7).Equals("connect"))
                        {
                            string ipForConnection = Strings.RemoveSpace(message.Substring(8));
                            IPEndPoint iP;
                            if (Strings.TryParseIp(ipForConnection, out iP))
                            {
                                client.Connect(iP);
                                isCommand = true;
                            }
                        }
                    }
                    else if (message.Substring(1).Equals("disconnect"))
                    {
                        client.Disconnect();
                        isCommand = true;
                    }
                    else if (message.Substring(1).Equals("interlocutors"))
                    {
                        List<IPEndPoint> interlocutors = client.ShowListOfConnections();
                        Console.WriteLine("*** Your connections ***");
                        foreach (var man in interlocutors)
                            Console.WriteLine($"{man}");
                        isCommand = true;
                    }
                    else if (message.Substring(1).Equals("exit"))
                    {
                        client.ExitFromChat();
                        isCommand = true;
                    }
                    else if (message.Substring(1).Equals("help"))
                    {
                        Console.WriteLine(Help());
                        isCommand = true;
                    }
                    if (!isCommand)
                        Console.WriteLine("*** Something went wrong. Uncorrect comand ***");
                }
                else
                    client.SendMessage(message);
            }
        }

        private int SelectionPort()
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

        private IPEndPoint SelectionIpByPort(int port)
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
    }
}
