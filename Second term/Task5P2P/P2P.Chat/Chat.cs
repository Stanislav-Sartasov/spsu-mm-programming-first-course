using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using P2P.Clients;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace P2P.Chat
{
    public class Chat
    {
        public void Registration(Client client)
        {
            IPEndPoint end = null;
            int port;
            bool isFreeIP = false;
            do
            {
                port = SetPort();
                client.SetLocalPoint(GetAddress(port));
                isFreeIP = client.BindSocket();
                if (!isFreeIP)
                    Console.WriteLine("This IP:port is already used. Try again.");
            }
            while (!isFreeIP);
            Console.WriteLine(GetAddress(port));
            client.SetClient();
            string info = "\n-info\n-clients\n-connect\n-disconnect\n-exit";
            Console.WriteLine(info);
            client.Listen();
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("-info"))
                    Console.WriteLine(info);
                else if (input.Equals("-clients"))
                {
                    foreach (EndPoint user in client.GetClients())
                        Console.WriteLine(user.ToString());
                }
                else if (input.Equals("-connect"))
                {
                    Console.WriteLine("Enter the IP:port you want to connect to ");
                    end = GetIPToConnect();
                    
                    client.Connect(end);
                }
                else if (input.Equals("-disconnect"))
                    client.Disconnect();
                else if (input.Equals("-exit"))
                    client.Exit();
                else
                    client.Send(input);
            }
              
        }
        
        private IPEndPoint GetAddress(int port)
        {
            IPEndPoint endPoint = null;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    endPoint = new IPEndPoint(ip, port);
                }
            }
            return endPoint;
        }
        private int SetPort()
        {
            Console.WriteLine("Enter your port");
            bool isCorrectPort = false;
            int port = 1;
            do
            {
                try
                {
                    port = int.Parse(Console.ReadLine());
                    if (port > 0 && port <= 65535)
                        isCorrectPort = true;
                    else
                    {
                        Console.WriteLine("Port ranging from 1 to 65535. Try again.");
                        isCorrectPort = false;
                    }
                        
                }
                catch
                {
                    Console.WriteLine("Enter a integer. Port ranging from 1 to 65535");
                    isCorrectPort = false;
                }
            }
            while (!isCorrectPort);
            
            return port;
        }
        private IPEndPoint GetIPToConnect()
        {
            IPEndPoint end = null;
            char[] delimiterChars = { '.', ':' };
            string[] elements;
            bool isCorrectFormat = false;
            do
            {

                string ip = Console.ReadLine();
                elements = ip.Split(delimiterChars);
                if (elements.Length != 5)
                {
                    
                    isCorrectFormat = false;

                }
                else
                {
                    if (elements[4] != "0")
                        isCorrectFormat = IPEndPoint.TryParse(ip, out end);
                    else
                        isCorrectFormat = false;
                    
                }
                if (!isCorrectFormat)
                    Console.WriteLine("Invalid format. IP:port looks like x.x.x.x:port, 0<=x<=255, 0<port<65536");

            }
            while (!isCorrectFormat);
            return end;
        }
    }

}
