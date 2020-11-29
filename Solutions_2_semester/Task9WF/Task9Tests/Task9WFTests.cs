using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Task9WF;
using Task9WF.Interfaces;
using Task9WF.Multithreaded;

namespace Task9Tests
{
	public class Tests
	{
		public class TestFront : IDataConsumer
		{
			public TaskManager TaskManager { get; set; }
			public event EventHandler<Message> NewInput;
			public event EventHandler Stopped;
			public bool Started { get; private set; } = false;

			List<string> messageList = new List<string>();
			Hashtable connectionList = new Hashtable();
			public int Port { get; set; } = -1;
			object locker = new object();
			public EndPoint Address
			{
				get
				{
					return new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
				}
			}
			public List<string> MessageList
			{
				get
				{
					lock(locker)
					{
						List<string> result = new List<string>();
						foreach (string s in messageList)
							result.Add(s);
						return result;
					}
				}
			}
			public Hashtable ConnectionList
			{
				get
				{
					lock (locker)
					{
						Hashtable result = new Hashtable();
						foreach (var s in connectionList.Keys)
							result[s] = connectionList[s];
						return result;
					}
				}
			}
			public int RequestStartPort()
			{
				return -1;
			}

			public void AddMessage(object sender, string message)
			{
				lock (locker)
				{
					messageList.Add(message);
				}
			}

			public void ChangeConnection(object sender, string name)
			{
				lock(locker)
				{
					connectionList[sender] = name;
				}
			}

			public void Start()
			{
				Started = true;
			}

			public void NewMessage(Message message)
			{
				Task.Run(() => NewInput(this, message));
			}

			public void Stop()
			{
				Stopped(this, EventArgs.Empty);
			}
		}

		[Test]
		public void ComplexTest()
		{
			const int ChatCount = 10;
			List<TestFront> testFronts = new List<TestFront>();
			List<Manager> managers = new List<Manager>();

			List<Guid> guidTable = new List<Guid>();

			for (int i = 0; i < ChatCount; i++)
			{
				TestFront front = new TestFront();
				Manager manager = new Manager();
				guidTable.Add(manager.Start(front, new TCPListener(), new SocketManager(), new Messager()));
				testFronts.Add(front);
				managers.Add(manager);
			}

			foreach (var guid in guidTable)
				Assert.AreEqual(1, guidTable.FindAll(x => x.Equals(guid)).Count);

			int[][] connectionsTable = new int[][]
				{
					new int[] { 1, 0 },
					new int[] { 2, 0 },
					new int[] { 0, 3 },
					new int[] { 1, 4 },
					new int[] { 5, 8 },
					new int[] { 6, 9 },
					new int[] { 7, 5 },
					new int[] { 8, 7 },
					new int[] { 8, 9 },
					new int[] { 9, 2 }
				};

			for (int i = 0; i < connectionsTable.Length; i++)
			{
				if (connectionsTable[i][0] >= ChatCount || connectionsTable[i][1] >= ChatCount)
					continue;
				testFronts[connectionsTable[i][0]].NewMessage(new Message() { Type = (byte)MessageType.Socket, Text = testFronts[connectionsTable[i][1]].Address.ToString() });
			}

			bool flag = false;

			for (int t = 0; t < 40; t++)
			{
				Thread.Sleep(5000);
				flag = true;
				
				foreach (TestFront front in testFronts)
					if (front.ConnectionList.Count != ChatCount - 1)
					{
						flag = false;
						break;
					}

				if (flag)
					break;
			}

			if (!flag)
				Assert.Fail();

			foreach (TestFront front in testFronts)
			{
				Hashtable connections = front.ConnectionList;
				foreach (Guid connection in connections.Keys)
					Assert.IsTrue(guidTable.Contains(connection));
			}

			int counter = 0;
			foreach (TestFront front in testFronts)
			{
				front.NewMessage(new Message() { Type = (byte)MessageType.Name, Text = counter.ToString() });
				front.NewMessage(new Message() { Type = (byte)MessageType.Message, Text = "Hello from " + counter.ToString() });
				counter++;
			}

			for (int t = 0; t < 40; t++)
			{
				Thread.Sleep(5000);

				flag = true;
				counter = 0;

				foreach (TestFront front in testFronts)
				{
					string messages = "";
					foreach (string s in front.MessageList)
						messages += s + "\n";

					for (int i = 0; i < testFronts.Count; i++)
						if (i != counter && !messages.Contains("Hello from " + i.ToString()))
						{
							flag = false;
							break;
						}

					if (!flag)
						break;

					counter++;
				}

				if (flag)
					break;
			}

			counter = 0;
			foreach (TestFront front in testFronts)
			{
				Console.WriteLine(counter);
				foreach (string s in front.MessageList)
					Console.WriteLine(s);
				Console.WriteLine();
				counter++;
			}

			if (!flag)
				Assert.Fail();
		}
	}
}