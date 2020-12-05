using Filter.AdditionLib;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Filter.Testing
{
    class Client
    {

        private volatile TcpClient client = null;
        IPEndPoint serverIp;
        volatile bool isContinued;
        public Client(string address)
        {
            isContinued = false;
            SecondaryFunctions.TryParseIp(address, out serverIp);
        }

        public void Stop()
        {
            isContinued = false;
            client.Close();
            client.Dispose();
        }
        public void Start(bool isSpam, int heightOfImage, int widthOfImage)
        {
            client = new TcpClient();
            client.Connect(serverIp);
            isContinued = true;
            NetworkStream networkStream = null;

            if (isSpam)
            {
                while (isContinued)
                {
                    try
                    {
                        SendImage(heightOfImage, widthOfImage);
                        if (client != null)
                            networkStream = client.GetStream();
                        networkStream.Flush();
                        while (networkStream.DataAvailable)
                        {
                            byte[] bytesToRead = new byte[1024];
                            networkStream.Read(bytesToRead, 0, 1024);
                        }
                        Thread.Sleep(900);
                    }
                    catch
                    {
                        if (client != null)
                        {
                            client.Close();
                            client.Dispose();
                            isContinued = false;
                        }
                    }
                }
                return;
            }

            SendImage(heightOfImage, widthOfImage);
            while (true)
            {
                if (client != null)
                    networkStream = client.GetStream();
                byte[] bufferLenOfMessage = new byte[4];
                networkStream.Read(bufferLenOfMessage, 0, 4);
                int lenOfMessage = BitConverter.ToInt32(bufferLenOfMessage, 0);
                byte[] bytesToRead = new byte[lenOfMessage - 4];

                List<byte> myData = new List<byte>(lenOfMessage);
                int amount = 0;
                int bytesRead = 0;
                while (amount < lenOfMessage - 4)
                {
                    bytesRead = networkStream.Read(bytesToRead, amount, lenOfMessage - 4 - amount);
                    amount += bytesRead;
                }
                myData.AddRange(bufferLenOfMessage);
                myData.AddRange(bytesToRead);
                if (MessageHandler(myData))
                {
                    client.Close();
                    return;
                }
            }
        }

        private void SendImage(int heightOfImage, int widthOfImage)
        {
            try
            {
                NetworkStream networkStream = null;
                if (client != null)
                {
                    networkStream = client.GetStream();

                    List<byte> sendingFilter = new List<byte>();
                    byte[] chosenFilter = SecondaryFunctions.TranslateToByteArrayUnicode("Gray", (byte)Protocol.Filter);
                    int lenFilter = 4 + chosenFilter.Length;
                    sendingFilter.AddRange(BitConverter.GetBytes(lenFilter));
                    sendingFilter.AddRange(chosenFilter);
                    if (client != null)
                    {
                        networkStream.Write(sendingFilter.ToArray(), 0, lenFilter);
                    }

                    byte[] image = new byte[heightOfImage * widthOfImage * 4];

                    int lenImage = 4 + 1 + 4 + 4 + heightOfImage * widthOfImage * 4;
                    List<byte> sendBytes = new List<byte>(lenImage);
                    sendBytes.AddRange(BitConverter.GetBytes(lenImage));
                    sendBytes.Add((byte)Protocol.Image);
                    sendBytes.AddRange(BitConverter.GetBytes(heightOfImage));
                    sendBytes.AddRange(BitConverter.GetBytes(widthOfImage));
                    sendBytes.AddRange(image);
                    if (client != null)
                    {
                        networkStream.Write(sendBytes.ToArray(), 0, lenImage);
                    }
                }
            }
            catch
            {
                //Console.WriteLine("Send image error");
                Stop();
            }
        }

        private bool MessageHandler(List<byte> myData)
        {
            while (myData.Count != 0)
            {
                int lenOfMessage = BitConverter.ToInt32(myData.ToArray(), 0);
                byte[] data = new byte[lenOfMessage - 4];
                myData.CopyTo(4, data, 0, lenOfMessage - 4);
                myData.RemoveRange(0, lenOfMessage);

                if (data[0] == (byte)Protocol.Image)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
