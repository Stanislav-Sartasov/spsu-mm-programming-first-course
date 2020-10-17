using Filter.Filtering;

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
        private bool isCalculating = false;
        private Thread thread;

        public ClientHandler(TcpClient client)
        {
            this.client = client;
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
                        data.AddRange(buffer);
                        bytes += temp;
                    }
                    while (temp != 0 && stream.DataAvailable);
                    if (bytes != 0)
                    {
                        data.RemoveRange(bytes, data.Count - bytes);
                        if (data[0] == 1)
                        {
                            chosenFilter = TranslateByteArrayToStringUnicode(data.ToArray());
                            continue;
                        }
                        if (data[0] == 2)
                        {
                            data.RemoveAt(0);
                            int height = BitConverter.ToInt32(data.ToArray(), 0);
                            int width = BitConverter.ToInt32(data.ToArray(), 4);
                            data.RemoveRange(0, 8);
                            isCalculating = true;
                            thread = new Thread(() => { Wait(data.ToArray(), height, width); });
                            thread.Start();
                            continue;
                        }
                        if (data[0] == 3 && isCalculating)
                            thread.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Disconnection");
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        private void Wait(byte[] data, int height, int width)
        {
            IFilter filter = Creator.Create(chosenFilter);

            Task<byte[]> temp = Task.Factory.StartNew<byte[]>(() => filter.Process(data, height, width));
            while (!temp.IsCompleted)
            {
                Thread.Sleep(1000);
                SendProgress((int)((filter.Progress() * 1.0) / height / width / 4 * 100));
            }
            SendProgress(100);
            Thread.Sleep(300); // против склейки буфера, исправить
            result = temp.Result;
            isCalculating = false;
            SendResult();
        }

        private void SendResult()
        {
            NetworkStream stream = null;
            stream = client.GetStream();
            byte[] buffer = new byte[1 + result.Length];
            buffer[0] = 1;
            for (int i = 0; i < result.Length; i++)
                buffer[i + 1] = result[i];
            stream.Write(buffer, 0, buffer.Length);
        }

        private void SendProgress(int v)
        {
            NetworkStream stream = null;
            stream = client.GetStream();
            byte[] buffer = new byte[2] { 2, (byte)v }; // 0 <= progress <= 100
            stream.Write(buffer, 0, 2);
        }

        public static string TranslateByteArrayToStringUnicode(byte[] message)
        {
            string result = Encoding.Unicode.GetString(message, 1, message.Length - 1); // message[0] - source

            return result;
        }
    }
}
