using Filter.Filtering;
using Filter.MagicConst;

using System;
using System.Collections.Generic;
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
        private byte[] result = null;
        private volatile bool isCalculating = false;
        private Thread processing;
        private string name;

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
                    byte[] buffer = new byte[256];
                    List<byte> data = new List<byte>(256);
                    int temp = 0;
                    int bytes = 0;
                    do
                    {
                        temp = stream.Read(buffer, 0, buffer.Length);
                        if (temp == 0)
                        {
                            Console.WriteLine("Disconnection " + name);
                            if (stream != null)
                                stream.Close();
                            if (client != null)
                                client.Close();
                        }
                        data.AddRange(buffer);
                        bytes += temp;
                    }
                    while (stream.DataAvailable);
                    if (bytes != 0)
                    {
                        data.RemoveRange(bytes, data.Count - bytes);
                        if (data[0] == (byte)Protocol.Filter)
                        {
                            chosenFilter = TranslateByteArrayToStringUnicode(data.ToArray());
                            continue;
                        }
                        if (data[0] == (byte)Protocol.Image)
                        {
                            data.RemoveAt(0);
                            int height = BitConverter.ToInt32(data.ToArray(), 0);
                            int width = BitConverter.ToInt32(data.ToArray(), 4);
                            data.RemoveRange(0, 8);
                            isCalculating = true;
                            processing = new Thread(() => { Wait(data.ToArray(), height, width); });
                            processing.Start();
                            continue;
                        }
                        if (data[0] == (byte)Protocol.Progress && isCalculating)
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
                    result = null;
                    chosenFilter = null;
                    tokenSource.Dispose();
                    return;
                }
                SendProgress((int)((filter.Progress() * 1.0) / height / width / 4 * 100));
            }
            SendProgress(100);
            Thread.Sleep(400);
            result = temp.Result;
            isCalculating = false;
            SendResult();
            result = null;
            chosenFilter = null;
            tokenSource.Dispose();
        }

        private void SendResult()
        {
            try
            {
                NetworkStream stream = null;
                stream = client.GetStream();
                byte[] buffer = new byte[1 + result.Length];
                buffer[0] = (byte)Protocol.Image;
                for (int i = 0; i < result.Length; i++)
                    buffer[i + 1] = result[i];
                stream.Write(buffer, 0, buffer.Length);
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
                stream.Write(buffer, 0, 2);
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
