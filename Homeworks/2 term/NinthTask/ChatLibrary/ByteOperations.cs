using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChatDescription
{
	public static class ByteOperations
	{
		public static byte[] MessageToBytes(string message, IPEndPoint currIP = null, List<IPEndPoint> ips = null) //ошибка
		{
			if (message[0] == '0')
			{
				return Encoding.Unicode.GetBytes(message);
			}
			else
			{
				var result = new List<byte> { (byte)message[0] };
				result.AddRange(IPToBytes(currIP.ToString()));
				result.Add((byte)'!');

				if (message[0] == '+')
				{
					foreach (var ip in ips)
					{
						result.AddRange(IPToBytes(ip.ToString()));
						result.Add((byte)'!');
					}
				}

				//
				Console.WriteLine("sending from" + currIP);
				foreach(var b in result)
				{
					Console.WriteLine(b);
				}
				Console.WriteLine("end_sending");
				//

				return result.ToArray();
			}
		}

		public static dynamic MessageFromBytes(List<byte> message, int p = 0) //дописать имена
		{
			try
			{
				message.RemoveAt(0);
				var temp = message.ToArray();

				if (p == 1)
				{
					return Encoding.Unicode.GetString(temp, 1, temp.Length - 1); //нумерация
				}
				else
				{
					var result = new List<IPEndPoint>();

					Console.WriteLine("receiving");
					Console.WriteLine(message.Count);
					for (int i = 0; i < message.Count; i++)
					{
						Console.WriteLine($"{message[i]} rec");
					}
					Console.WriteLine("end_receiving");

					for (int i = 0; i < temp.Length; i += 7)
					{
						result.Add(new IPEndPoint(BitConverter.ToUInt32(temp, i), BitConverter.ToUInt16(temp, i + 4)));
					}

					return result;
				}
			}
			catch
			{
				Console.WriteLine("Error in bytes decoding!");
				return null;
			}
		}

		private static List<byte> IPToBytes(string ip) //переделать
		{
			var result = new List<byte>();
			var temp = ip.Split('.', ':');

			//Console.WriteLine("resulting");
			for (int i = 0; i < 5; i++)
			{
				if (i != 4)
				{
					result.Add(byte.Parse(temp[i]));
				}
				else
				{
					result.AddRange(BitConverter.GetBytes(ushort.Parse(temp[i])));
				}

				//Console.WriteLine(result.Count);
			}
			//Console.WriteLine("end_resulting");

			return result;
		}
	}
}
