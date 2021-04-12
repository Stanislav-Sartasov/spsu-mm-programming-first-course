using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Protocols;
using System.Drawing;
using System.IO;
using System.Collections;

namespace Task7Server
{
	public class Server
	{
		public Server(string adress, int port, IFilter[] filters)
		{
			foreach (var f in filters)
			{
				if (this.filters.ContainsKey(f.Name))
					throw new Exception("filter names cannot be repeated");

				this.filters[f.Name] = f;
				if (filtersString != null)
					filtersString += ":" + f.Name;
				else
					filtersString = f.Name;
			}

			listener = new TcpListener(IPAddress.Parse(adress), port);
		}

		volatile TcpListener listener = null;
		volatile int working = 0;
		volatile bool stopped = false;
		volatile List<Task> activeTasks = new List<Task>();
		volatile Hashtable filters = new Hashtable();
		volatile string filtersString = null;


		public void Start() 
		{
			try
			{
				if (Interlocked.CompareExchange(ref working, 1, 0) == 0)
				{
					listener.Start(128);

					Console.WriteLine($"started at: {listener.LocalEndpoint}");

					while (!stopped)
					{
						TcpClient client = listener.AcceptTcpClient();

						lock (activeTasks)
							if (!stopped)
							{
								for (int i = 0; i < activeTasks.Count; i++)
									if (activeTasks[i].IsCompleted)
										activeTasks.RemoveAt(i);
									else
										i++;
								activeTasks.Add(Task.Run(() => Work(client)));
							}
					}

				}
			}
			catch (Exception ex)
			{
				if (!stopped)
				{
					working = 0;
					throw ex;
				}
			}

			working = 0;
		}

		void Work(TcpClient client)
		{
			EndPoint address = null;
			try
			{
				address = client.Client.RemoteEndPoint;
				Console.WriteLine($"{address}: connected");

				Message message = Message.GetFromStream(client, 5000);

				switch ((SendingProtocols)message.Flag)
				{
					case SendingProtocols.Check:
						Console.WriteLine($"{address}: check");
						Check(client);
						break;
					case SendingProtocols.FilterSending:
						Console.WriteLine($"{address}: sending filters");
						FilterSend(client);
						break;
					case SendingProtocols.BitMapSending:
						Console.WriteLine($"{address}: {message.Content}");
						UseFilter(client, message);
						break;
					default:
						throw new Exception();
				}
				
				client.Close();
				Console.WriteLine($"{address}: finished");
			}
			catch (Exception ex)
			{
				try
				{
					client.Close();
				}
				catch { }
				if (address != null && !stopped)
					Console.WriteLine($"{address}: failed\n{ex}");
				return;
			}
		}

		void Check(TcpClient client)
		{
			new Message(SendingProtocols.Check).SendToStream(client, 5000);
		}
		void FilterSend(TcpClient client)
		{
			new Message(filtersString).SendToStream(client, 5000);
		}
		void UseFilter(TcpClient client, Message message)
		{
			IFilter filter = null;
			try
			{
				string key = message.Content;

				lock (filters)
					if (!filters.ContainsKey(key))
						return;

				message = Message.GetFromStream(client, 5000);

				lock (filters)
					filter = ((IFilter)filters[key]).Use(message.ContentByteArray);

				while (true)
				{
					Thread.Sleep(1000);
					double progress = filter.Progress;
					if (progress >= 100)
						break;
					else if (progress == -1 || stopped)
						return;

					new Message((int)progress).SendToStream(client, 5000);
				}

				new Message(100, filter.Result).SendToStream(client, 5000);
				Message.GetFromStream(client, 5000);
			}
			catch (Exception ex)
			{
				if (filter != null)
					filter.Abort();
				throw ex;
			}
		}

		public void Dispose()
		{
			stopped = true;

			if (listener != null)
				listener.Stop();

			while (working != 0)
				Thread.Sleep(10);

			Task[] taskArr;
			lock (activeTasks)
				taskArr = activeTasks.ToArray();

			Task.WaitAll(taskArr);
		}

		~Server()
		{
			Dispose();
		}
	}
}
