using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChatDescription
{
	public class ChatManager
	{
		public Client CurrClient { get; private set; }

		public void StartChating()
		{
			Console.WriteLine(">Welcome to chat!");
			ShowInfo();

			SetClientConnection();

			GetClientData();
		}

		private void ShowInfo()
		{
			Console.WriteLine("\n>Commands: \n>/connect <ip> - connect to other client (<ip> is X.X.X.X:Y; 0 <= X <= 255, 1 <= Y <= 65535).\n>/disconnect - disconnect from all other connected clients.\n>/connections - show your curent connections.\n>/help - show commands.\n>/exit - close chat.\n");
		}

		private void SetClientConnection()
		{
			while (true)
			{
				try
				{
					Console.WriteLine(">Input your port:");
					var port = GetClientData();

					foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
					{
						if (ip.AddressFamily == AddressFamily.InterNetwork)
						{
							CurrClient.TryGetAddress(new IPEndPoint(ip, port));
							break;
						}
					}
					break;
				}
				catch
				{
					Console.WriteLine(">This port is already used, try another one.");
				}
			}

			CurrClient.StartListening();
		}

		private int GetClientData()
		{
			while (true)
			{
				if (CurrClient.IsConnected == 0)
				{
					try
					{
						var input = CurrClient.GetInput();

						if (input == "/exit")
						{
							CurrClient.Exit();
						}

						var port = int.Parse(input);

						if (port >= 1 && port <= 65535)
						{
							return port;
						}
						throw new Exception();
					}
					catch
					{
						Console.WriteLine(">Invalid port value, please input a number from 1 to 65535.");
					}

				}
				else
				{
					Console.Write(">");

					var input = CurrClient.GetInput();
					if (input == "")
					{
						continue;
					}

					if (input[0] == '/')
					{
						try
						{
							input = input.ToLower().Remove(0, 1);
							var str = input.Split(' ');

							if (str[0] == "connect")
							{
								CurrClient.Connect(TryParseIP(str));

							}
							else if (str.Length > 1)
							{
								throw new Exception("Unknown comand, probably incorrect input.\nPlease try again or use /help");
							}
							else
							{
								switch (str[0])
								{
									case "disconnect":
										CurrClient.Disconnect();
										break;
									case "connections":
										Console.WriteLine(">Your connections now:");

										var ips = new List<IPEndPoint>(CurrClient.UserList);
										foreach (var ip in ips)
										{
											Console.WriteLine($">>{ip}");
										}
										break;
									case "help":
										ShowInfo();
										break;
									case "exit":
										CurrClient.Exit();
										break;
									default:
										throw new Exception(">Unknown comand, probably incorrect input.\nPlease try again or use /help");
								}
							}
						}
						catch(Exception ex)
						{
							Console.WriteLine(ex.Message);
						}

					}
					else
					{
						CurrClient.SendMessage(ByteOperations.MessageToBytes("0" + input));
					}
				}
			}
		}

		private static IPEndPoint TryParseIP(string[] msg)
		{
			try
			{
				if (msg.Length > 2)
				{
					throw new Exception();
				}

				var temp = msg[1].Split(new char[] { '.', ':' });
				if (temp.Length != 5)
				{
					throw new Exception();
				}

				var ip = new List<byte>();
				for (int i = 0; i < 4; i++)
				{
					ip.Add(byte.Parse(temp[i]));
				}

				return new IPEndPoint(BitConverter.ToUInt32(ip.ToArray()), ushort.Parse(temp[4]));
				
			}
			catch
			{
				throw new Exception(">Can't connect to address.\nProbably incorrect input - please, use /help.");
			}
		}

		public ChatManager(Client client)
		{
			CurrClient = client;
		}
	}
}
