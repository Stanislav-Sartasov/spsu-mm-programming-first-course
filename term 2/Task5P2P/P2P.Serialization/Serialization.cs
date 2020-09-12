using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace P2P.Serialization
{
    public class Serialization
    {
        public static byte[] Serialize(List<EndPoint> clients, char key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Insert(0, key);
            foreach (EndPoint client in clients)
            {
                stringBuilder.Append(client.ToString());
                stringBuilder.Append(' ');
            }
            byte[] data = Encoding.Unicode.GetBytes(stringBuilder.ToString());
            return data;
        }

        public static List<EndPoint> Deserialize(StringBuilder stringBuilder)
        {
            char[] temp = new char[stringBuilder.Capacity];
            stringBuilder.CopyTo(1, temp, stringBuilder.Length - 1);
            string users = new string(temp);
            string[] tempClients = users.Split(' ');

            IPEndPoint end = null;
            List<EndPoint> clients = new List<EndPoint>();
            for (int i = 0; i < tempClients.Length - 1; i++)
            {
                IPEndPoint.TryParse(tempClients[i], out end);
                clients.Add(end);
            }
            return clients;
        }
    }
}
