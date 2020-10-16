using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Task9WF;
using Task9WF.Interfaces;

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
            EndPoint address = null;
            public EndPoint Address
            {
                get
                {
                    lock (locker)
                    {
                        return address;
                    }
                }
            }
            object locker = new object();
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
                        foreach (EndPoint s in connectionList.Keys)
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
                    if (address == null && message.Contains("Server started at: "))
                    {
                        int i = message.IndexOf("Server started at: ");
                        string addr = message.Substring(i + "Server started at: ".Length);
                        address = new Message() { Type = MessageType.Socket, Text = addr }.GetAddress();
                    }
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
                NewInput(this, message);
            }

            public void Stop()
            {
                Stopped(this, EventArgs.Empty);
            }
        }

        [Test]
        public void ComplexTest()
        {
            const int ChatCount = 4;
            List<TestFront> testFronts = new List<TestFront>();
            List<Manager> managers = new List<Manager>();

            for (int i = 0; i < ChatCount; i++)
            {
                TestFront front = new TestFront();
                Manager manager = new Manager();
                manager.Start(front, new TCPListener(), new SocketManager(), new Messager());
                testFronts.Add(front);
                managers.Add(manager);
            }

            List<EndPoint> table = new List<EndPoint>();

            foreach (TestFront front in testFronts)
            {
                Assert.IsFalse(table.Contains(front.Address));
                table.Add(front.Address);
            }

            int[][] connectionsTable = new int[][]
                {
                    new int[] { 1, 0 },
                    new int[] { 2, 0 },
                    new int[] { 0, 3 },
                    new int[] { 1, 4 },
                };

            for (int i = 0; i < ChatCount - 1; i++)
            {
                testFronts[connectionsTable[i][0]].NewMessage(new Message() { Type = MessageType.Socket, Text = testFronts[connectionsTable[i][1]].Address.ToString() });
                Thread.Sleep(10000);
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

            Thread.Sleep(12000);

            int counter = 0;
            foreach (TestFront front in testFronts)
            {
                Console.WriteLine(counter);
                foreach (string s in front.MessageList)
                    Console.WriteLine(s);
                Console.WriteLine();
                counter++;
            }
            Assert.Fail();          //to fix a bug with connections

            if (!flag)
                Assert.Fail();

            foreach (TestFront front in testFronts)
            {
                Hashtable connections = front.ConnectionList;
                foreach (EndPoint connection in connections.Keys)
                    Assert.IsTrue(table.Contains(connection));
            }

            counter = 0;
            foreach (TestFront front in testFronts)
            {
                front.NewMessage(new Message() { Type = MessageType.Name, Text = counter.ToString() });
                front.NewMessage(new Message() { Type = MessageType.Message, Text = "Hello from " + counter.ToString() });
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

            if (!flag)
            {
                counter = 0;
                foreach (TestFront front in testFronts)
                {
                    Console.WriteLine(counter);
                    foreach (string s in front.MessageList)
                        Console.WriteLine(s);
                    Console.WriteLine();
                    counter++;
                }
                Assert.Fail();
            }

            counter = 0;
            foreach (TestFront front in testFronts)
            {
                Hashtable connections = front.ConnectionList;
                for (int i = 0; i < testFronts.Count; i++)
                    if (i != counter)
                        Assert.IsTrue(((HashSet<string>)connections.Values).Contains(i.ToString()));
            }
        }
    }
}