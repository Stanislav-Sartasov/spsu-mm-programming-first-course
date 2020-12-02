using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserInterface;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace UserLibrary
{
    //TODO EndPointToByteArray, ByteArrayToEndPoint, ListEndPointToByteArray, ByteArrayToListEndPoint
    public static class ArrayMethods
    {
        public static byte[] ConcatArrays(byte[] firstArray, byte[] secondArray)
        {
            
            if (firstArray == null)
                return secondArray;
            if (secondArray == null)
                return firstArray;

            byte[] buffer = new byte[firstArray.Length + secondArray.Length];
            firstArray.CopyTo(buffer, 0);
            secondArray.CopyTo(buffer, firstArray.Length);
            return buffer;
        }

        public static byte[] StringToArrayOfByte(string input)
        {
            return Encoding.ASCII.GetBytes(input);
        }

        public static string ArrayOfByteToString(byte[] array)
        {
            return Encoding.ASCII.GetString(array);
        }

        public static byte[] EndPointToByteArray(EndPoint userIp)
        {
            List<byte> result = new List<byte>();
            string address = userIp.ToString();
            int i = 0;
            for (int j = 0; j < 4; i++, j++)
            {
                StringBuilder ip = new StringBuilder("");
                while (address[i] != '.' && address[i] != ':')
                {
                    ip.Append(address[i]);
                    i++;
                }
                result.Add(byte.Parse(ip.ToString()));
            }
            result.AddRange(BitConverter.GetBytes(UInt16.Parse(address.Substring(i))));
            return result.ToArray();
        }

        public static EndPoint ByteArrayToEndPoint(byte[] array)
        {
            UInt32 ip = BitConverter.ToUInt32(array, 0);
            UInt16 port = BitConverter.ToUInt16(array, 4);
            return new IPEndPoint(ip, port);
        }

        public static byte[] ListEndPointToByteArray(List<EndPoint> listEndPoint)
        {
            List<byte> result = new List<byte>() { };
            foreach (IPEndPoint interlocutor in listEndPoint)
                result.AddRange(EndPointToByteArray(interlocutor));
            return result.ToArray();
        }

        public static List<EndPoint> ByteArrayToListEndPoint(byte[] listOfAddresses)
        {
            List<EndPoint> result = new List<EndPoint>();

            if (listOfAddresses.Length % 6 == 0)
                for (int i = 0; i < listOfAddresses.Length; i += 6)
                    result.Add(new IPEndPoint(BitConverter.ToUInt32(listOfAddresses, i), BitConverter.ToUInt16(listOfAddresses, i + 4)));
            else
                return null;

            return result;
        }
    }
}
