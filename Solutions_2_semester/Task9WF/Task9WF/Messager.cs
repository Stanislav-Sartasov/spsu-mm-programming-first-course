using System;
using System.Text;
using Task9WF.Interfaces;
using System.Net.Sockets;

namespace Task9WF
{
    public class Messager : IMessager
    {
        public Messager() { }
        public Messager(int packageSize)
        {
            if (packageSize > 0)
                this.packageSize = packageSize;
        }

        int packageSize = 32;
        public bool SendMessage(Socket socket, MessageType type, string message)
        {
            try
            {
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message + "$");
                int size = (messageBuffer.Length + 5) / packageSize + ((messageBuffer.Length + 5) % packageSize == 0 ? 0 : 1);

                byte[] output = new byte[packageSize * size];

                Array.Copy(BitConverter.GetBytes(size), output, 4);
                output[4] = (byte)type;

                Array.Copy(messageBuffer, 0, output, 5, messageBuffer.Length);

                socket.Send(output);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Message ReceiveMassege(Socket socket)
        {
            try
            {
                Message message = new Message() { Text = "", Type = MessageType.Message };
                byte[] buffer;
                byte[] messageBuffer = new byte[0];
                bool full = false;
                bool started = false;
                int size = 0;
                int sizeNow = 0;

                while (!full)
                {
                    buffer = new byte[packageSize];
                    socket.Receive(buffer);

                    int startIndex = 0;

                    if (!started)
                    {
                        size = BitConverter.ToInt32(buffer, 0);
                        message.Type = (MessageType)buffer[4];
                        messageBuffer = new byte[packageSize * size - 5];
                        startIndex = 5;
                        started = true;
                    }

                    Array.Copy(buffer, startIndex, messageBuffer, packageSize * sizeNow - 5 + startIndex, packageSize - startIndex);

                    sizeNow++;

                    if (sizeNow >= size)
                    {
                        message.Text = Encoding.UTF8.GetString(messageBuffer);
                        while (message.Text[message.Text.Length - 1] == '\0')
                            message.Text = message.Text.Substring(0, message.Text.Length - 1);
                        message.Text = message.Text.Substring(0, message.Text.Length - 1);
                        full = true;
                    }
                }

                return message;
            }
            catch
            {
                return new Message { Type = MessageType.Name, Text = null };
            }
        }
    }
}
