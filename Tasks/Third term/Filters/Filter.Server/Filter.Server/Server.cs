using Filter.Filtering;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Filter.Server
{
    class Server
    {
        // отправляемые: 1 - картинка, 2 - прогресс, 3 - filters
        // получаемые: 1 - filter, 2 - image, 3 - stop
        const int port = 8000;
        static TcpListener listener;
        static void Main(string[] args)
        {
            Creator.Initialize();
            try
            {
                IPEndPoint temp = SelectionIpByPort(port);
                Console.WriteLine(temp);
                listener = new TcpListener(temp);
                
                listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Connection");
                    SendFilters(client);
                    ClientHandler clientHandler = new ClientHandler(client);

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
            byte[] buffer = TranslateToByteArrayUnicode(Creator.AvailableFilters(), 3);

            stream.Write(buffer, 0, buffer.Length);
        }

        private static byte[] TranslateToByteArrayUnicode(string message, byte source) // source = 3
        {
            byte[] temp = Encoding.Unicode.GetBytes(message.ToString());
            byte[] result = new byte[temp.Length + 1];
            result[0] = source; // 3 - filters
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
