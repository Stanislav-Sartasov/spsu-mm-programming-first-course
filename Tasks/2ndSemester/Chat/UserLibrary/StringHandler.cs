using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserInterface;
using System.Net;
using System.IO;
using System.Linq;

namespace UserLibrary
{
    public static class StringHandler
    {
        public static void InputHandler(ICommand user, string input)
        {
            string[] words = input.Split(' ');
            switch (words[0])
            {
                case "\\connect":
                    if (StringToIp(words[1], out IPEndPoint ip))
                        user.Connect(ip);
                    else
                        Console.WriteLine("Invalid input. Try again.");
                    break;
                case "\\disconnect": user.Disconnect(); break;
                case "\\exit": user.Exit(); break;
                default: user.SendMessage(input); break;
            }
        }

        public static void MessageHandler(IMessage user, byte[] input)
        {
            switch (input[0])
            {
                case (byte)Protocol.Message: user.WriteMessage((string)ArrayMethods.ArrayOfByteToString(input.Skip(1).ToArray())); break;
                case (byte)Protocol.Connect: user.ConnectMessage((EndPoint)ArrayMethods.ByteArrayToEndPoint(input.Skip(1).ToArray())); break;
                case (byte)Protocol.ConnectAnswer: user.ConnectAnswerMessage((List<EndPoint>)ArrayMethods.ByteArrayToListEndPoint(input.Skip(1).ToArray())); break;
                case (byte)Protocol.Disconnect: user.DisconnectMessage((EndPoint)ArrayMethods.ByteArrayToEndPoint(input.Skip(1).ToArray())); break;
                default: Console.WriteLine("Something get wrong. MessageHandler error."); break;
            }
        }

        private static bool StringToIp(string input, out IPEndPoint ip)
        {
            string[] IpPort = input.Split(':');
            try
            {
                ip = new IPEndPoint(IPAddress.Parse(IpPort[0]), Convert.ToInt32(IpPort[1]));
                return true;
            }
            catch
            {
                ip = null;
                return false;
            }
        }
    }
}
