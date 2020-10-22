using Filter.Filtering;
using Filter.MagicConst;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Filter.Server
{
    public class ClientHandler
    {
        private TcpClient client;
        private string chosenFilter;
        private volatile bool isCalculating = false;
        private Thread processing;
        private readonly string name;

        public ClientHandler(TcpClient client, string name)
        {
            this.client = client;
            this.name = name;
        }
        public void Listen()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                while (true)
                {
                    byte[] bufferLenOfInput = new byte[4];
                    stream.Read(bufferLenOfInput, 0, 4);
                    int lenOfInput = BitConverter.ToInt32(bufferLenOfInput, 0);
                    byte[] bytesToRead = new byte[lenOfInput - 4];

                    List<byte> storage = new List<byte>(lenOfInput);
                    int amount = 0;
                    int bytesRead = 0;
                    while (amount < lenOfInput - 4)
                    {
                        bytesRead = stream.Read(bytesToRead, amount, lenOfInput - 4 - amount);
                        amount += bytesRead;
                    }
                    storage.AddRange(bufferLenOfInput);
                    storage.AddRange(bytesToRead);

                    while (storage.Count != 0)
                    {
                        int lenOfMessage = BitConverter.ToInt32(storage.ToArray(), 0);
                        byte[] data = new byte[lenOfMessage - 4];
                        storage.CopyTo(4, data, 0, lenOfMessage - 4);
                        storage.RemoveRange(0, lenOfMessage);

                        if (data[0] == (byte)Protocol.Filter)
                        {
                            chosenFilter = TranslateByteArrayToStringUnicode(data);
                            continue;
                        }
                        if (data[0] == (byte)Protocol.Image)
                        {
                            data = data.Skip(1).ToArray();
                            int height = BitConverter.ToInt32(data, 0);
                            int width = BitConverter.ToInt32(data, 4);
                            data = data.Skip(8).ToArray();
                            isCalculating = true;
                            processing = new Thread(() => { Wait(data, height, width); });
                            processing.Start();
                            continue;
                        }
                        if (data[0] == (byte)Protocol.Progress)
                            if (2 <= data.Length && data[1] == (byte)Protocol.StopCode && isCalculating)
                                isCalculating = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(name + " said: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Disconnection " + name);
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        private void Wait(byte[] data, int height, int width)
        {
            IFilter filter = Creator.Create(chosenFilter);
            if (filter == null)
            {
                SendProgress((int)Protocol.StopCode);
                return;
            }
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task<byte[]> temp = Task.Factory.StartNew<byte[]>(() => filter.Process(data, height, width, token));
            while (!temp.IsCompleted)
            {
                Thread.Sleep(1000);
                if (!isCalculating)
                {
                    tokenSource.Cancel();
                    SendProgress((int)Protocol.StopCode);
                    chosenFilter = null;
                    tokenSource.Dispose();
                    return;
                }
                SendProgress((int)((filter.Progress() * 1.0) / height / width / 4 * 100));
            }
            SendProgress(100);
            byte[] result = temp.Result;
            isCalculating = false;
            SendResult(result);
            chosenFilter = null;
            tokenSource.Dispose();
        }

        private void SendResult(byte[] result)
        {
            try
            {
                NetworkStream stream = null;
                stream = client.GetStream();
                byte[] buffer = new byte[1 + result.Length];
                buffer[0] = (byte)Protocol.Image;
                for (int i = 0; i < result.Length; i++)
                    buffer[i + 1] = result[i];

                int len = 4 + buffer.Length;
                List<byte> sendBytes = new List<byte>();
                sendBytes.AddRange(BitConverter.GetBytes(len));
                sendBytes.AddRange(buffer);
                stream.Write(sendBytes.ToArray(), 0, len);
            }
            catch (Exception ex)
            {
                Console.WriteLine(name + " said: " + ex.Message);
            }
        }

        private void SendProgress(int v)
        {
            try
            {
                NetworkStream stream = null;
                stream = client.GetStream();
                byte[] buffer = new byte[2] { (byte)Protocol.Progress, (byte)v }; // 0 <= progress <= 100
                int len = 4 + buffer.Length;
                List<byte> sendBytes = new List<byte>();
                sendBytes.AddRange(BitConverter.GetBytes(len));
                sendBytes.AddRange(buffer);
                stream.Write(sendBytes.ToArray(), 0, len);
            }
            catch (Exception ex)
            {
                Console.WriteLine(name + " said: " + ex.Message);
            }
        }

        public static string TranslateByteArrayToStringUnicode(byte[] message)
        {
            string result = Encoding.Unicode.GetString(message, 1, message.Length - 1); // message[0] - source

            return result;
        }
    }
}
