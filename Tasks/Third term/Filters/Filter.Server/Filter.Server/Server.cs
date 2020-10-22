using Filter.Filtering;
using Filter.MagicConst;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Filter.Server
{
    class Server
    {
        const int port = 8000;
        static TcpListener listener;
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to .cfg file (or leave the line blank for the default)");
            string path;
            path = Console.ReadLine();
            if (path.Equals(""))
                path = Directory.GetCurrentDirectory() + @"\.." + @"\.." + @"\.." + @"\.." + "/Filters.cfg";

            Creator.Initialize(path);
            int count = 0;
            try
            {
                IPEndPoint temp = SelectionIpByPort(port);
                Console.WriteLine(temp);
                listener = new TcpListener(temp);
                
                listener.Start();
                Console.WriteLine("***Start***");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    count++;
                    Console.WriteLine($"Connection {count}");
                    SendFilters(client);
                    ClientHandler clientHandler = new ClientHandler(client, $"Client {count}");

                    Thread clientThread = new Thread(clientHandler.Listen);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        private static void SendFilters(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = TranslateToByteArrayUnicode(Creator.AvailableFilters(), (byte)Protocol.Filter);
            int len = 4 + buffer.Length;
            List<byte> sendBytes = new List<byte>();
            sendBytes.AddRange(BitConverter.GetBytes(len));
            sendBytes.AddRange(buffer);
            stream.Write(sendBytes.ToArray(), 0, len);
        }

        private static byte[] TranslateToByteArrayUnicode(string message, byte source)
        {
            byte[] temp = Encoding.Unicode.GetBytes(message.ToString());
            byte[] result = new byte[temp.Length + 1];
            result[0] = source;
            Array.Copy(temp, 0, result, 1, temp.Length);
            return result;
        }

        private static IPEndPoint SelectionIpByPort(int port)
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
