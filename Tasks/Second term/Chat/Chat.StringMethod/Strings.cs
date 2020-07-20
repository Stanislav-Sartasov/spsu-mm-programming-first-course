using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Chat.StringMethod
{
    public static class Strings
    {
        public static void WriteIp(IPEndPoint Ip)
        {
            Console.WriteLine($"*** Your IP in network: {Ip.Address}:{Ip.Port} ***");
        }

        public static bool CompareIp(IPEndPoint firstIp, IPEndPoint secondIp)
        {
            if (firstIp.Equals(secondIp))
            {
                Console.WriteLine("*** This is your IP. ***");
                return true;
            }
            return false;
        }

        public static List<byte> ParseAddressToByteList(IPEndPoint userIp)
        {
            List<byte> result = new List<byte>();
            string address = $"{userIp}";
            int i = 0;
            for (int j = 0; j < 4; j++)
            {
                StringBuilder ip = new StringBuilder("");
                while (address[i] != '.' && address[i] != ':')
                {
                    ip.Append(address[i]);
                    i++;
                }
                i++;
                result.Add(byte.Parse(ip.ToString()));
            }
            UInt16 port = UInt16.Parse(address.Substring(i));
            result.AddRange(BitConverter.GetBytes(port));
            return result;
        }

        public static List<IPEndPoint> ParseByteListToAddress(byte[] listOfAddresses)
        {
            // address = 6 byte
            List<IPEndPoint> result = new List<IPEndPoint>();

            if (listOfAddresses.Length % 6 == 0)
                for (int i = 0; i < listOfAddresses.Length; i += 6)
                {
                    UInt32 ip = BitConverter.ToUInt32(listOfAddresses, i);
                    UInt16 port = BitConverter.ToUInt16(listOfAddresses, i + 4);
                    result.Add(new IPEndPoint(ip, port));
                }
            else
            {
                Console.WriteLine("Something wrong, ParseByteListToAddress method");
                return null;
            }
            return result;
        }

        public static byte[] TranslateToByteArray(IPEndPoint userIp, List<IPEndPoint> listOfUsersConnections, byte source)
        {
            List<byte> result = new List<byte>() { source };
            result.AddRange(ParseAddressToByteList(userIp));
            foreach (IPEndPoint interlocutor in listOfUsersConnections)
                result.AddRange(ParseAddressToByteList(interlocutor));
            return result.ToArray();
        }

        public static byte[] TranslateToByteArray(IPEndPoint userIp, byte source)
        {
            List<byte> result = new List<byte>() { source };
            result.AddRange(ParseAddressToByteList(userIp));
            return result.ToArray();
        }

        public static byte[] TranslateToByteArrayUnicode(string message, byte source)
        {
            byte[] temp = Encoding.Unicode.GetBytes(message.ToString());
            byte[] result = new byte[temp.Length + 1];
            result[0] = source; // 0 - user, 1 - connect, 2 - disconnect
            Array.Copy(temp, 0, result, 1, temp.Length);
            return result;
        }
        
        public static string TranslateByteArrayToStringUnicode(byte[] message)
        {
            string result = Encoding.Unicode.GetString(message, 1, message.Length - 1); // message[0] - source

            return result;
        }
        public static bool TryParseIp(string ipForConnection, out IPEndPoint iP)
        {
            string[] temp = ipForConnection.Split(new char[] { '.', ':' });
            if (temp.Length != 5)
            {
                iP = null;
                return false;
            }
            else
            {
                for (int k = 0; k < 5; k++)
                {
                    foreach (char c in temp[k])
                        if (c < '0' || '9' < c)
                        {
                            iP = null;
                            return false;
                        }
                }
                List<byte> result = new List<byte>();
                for (int j = 0; j < 4; j++)
                    result.Add(byte.Parse(temp[j]));
                UInt32 ip = BitConverter.ToUInt32(result.ToArray(), 0);
                UInt16 port = UInt16.Parse(temp[4]);
                iP = new IPEndPoint(ip, port);
                return true;
            }
        }
        public static string RemoveSpace(string v)
        {
            StringBuilder result = new StringBuilder();
            foreach (var i in v)
            {
                if (i != ' ')
                    result.Append(i);
            }
            return result.ToString();
        }
    }
}