using System;
using System.Collections.Generic;
using System.Net;

namespace Task9WF
{
	public class Info
	{
		Guid guid = Guid.Empty;
		IPAddress[] iPAddresses = null;
		int port = -1;
		object locker = new object();
		public Guid Guid
		{
			get
			{
				lock (locker)
				{
					return guid;
				}
			}
			set
			{
				lock (locker)
				{
					if (guid.Equals(Guid.Empty))
						guid = value;
				}
			}
		}
		public IPAddress[] IPAddresses
		{
			get
			{
				lock (locker)
				{
					return iPAddresses;
				}
			}
			set
			{
				lock (locker)
				{
					if (iPAddresses == null)
						iPAddresses = value;
				}
			}
		}
		public int Port
		{
			get
			{
				lock (locker)
				{
					return port;
				}
			}
			set
			{
				lock (locker)
				{
					if (port < 0)
						port = value;
				}
			}
		}
		public override string ToString()
		{
			lock (locker)
			{
				string result = guid.ToString() + '|' + port.ToString();
				foreach (IPAddress address in iPAddresses)
					result += '|' + address.ToString();
				return result;
			}
		}
		public static Info Parse(string s)
		{
			try
			{
				string[] input = s.Split('|');

				Info info = new Info
				{
					Guid = Guid.Parse(input[0]),
					Port = int.Parse(input[1])
				};

				List<IPAddress> iPAddresses = new List<IPAddress>();
				for (int i = 2; i < input.Length; i++)
					iPAddresses.Add(IPAddress.Parse(input[i]));
				info.IPAddresses = iPAddresses.ToArray();

				return info;
			}
			catch
			{
				return null;
			}
		}
		public bool Equals(Info info)
		{
			return ToString().Equals(info.ToString());
		}
	}
}