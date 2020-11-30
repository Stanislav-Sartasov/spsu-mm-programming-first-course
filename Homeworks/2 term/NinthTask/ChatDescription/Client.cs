using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NinthTask.ChatDescription
{
        class Client
        {
                Thread ThreadReadServer;
                void GetReadServ() // Server data monitoring
                {
                        while (ChatManager.FlagWorking)
                        {
                                ChatManager.MutexSocketServer.WaitOne();
                                if (ChatManager.SocketServer.Available > 0)
                                {
                                        byte[] bt = new byte[1024];
                                        int sz = ChatManager.SocketServer.Receive(bt);
                                        int code = BitConverter.ToInt32(bt, 0);

                                        Console.WriteLine(code);

                                        if (code == 3)
                                        {
                                                int len = BitConverter.ToInt32(bt, 4);
                                                string data = Encoding.ASCII.GetString(bt, 8, len);
                                                Console.WriteLine(data);
                                                ChatManager.ForwardOnlyUsers(bt, 8 + len);
                                        }
                                        if (code == 2)
                                        {
                                                int len = BitConverter.ToInt32(bt, 4);
                                                string data = Encoding.ASCII.GetString(bt, 8, len);
                                                Console.WriteLine("{0}> {1}", ChatManager.SocketServer.RemoteEndPoint.ToString(), data);
                                                ChatManager.SendOnlyUsers(ChatManager.SocketServer.RemoteEndPoint.ToString(), data);
                                        }
                                }
                                ChatManager.MutexSocketServer.ReleaseMutex();
                                Thread.Sleep(0);
                        }
                }
                public void Connect(string address, int port)
                {
                        ChatManager.SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        //string address = "127.0.0.1";
                        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                        ChatManager.SocketServer.Connect(ipPoint);
                        Console.WriteLine("Address Local: " + ChatManager.SocketServer.LocalEndPoint.ToString());
                        ChatManager.SocketServer.Send(BitConverter.GetBytes(0));
                }
                public void Start()
                {
                        ThreadReadServer = new Thread(GetReadServ);
                        ThreadReadServer.Start();
                }
                public void Join()
                {
                        ThreadReadServer.Join();
                }
               
                public void SendServer(byte[] msg, int size) // Sending message to server (if it exists) and all users
                {
                        ChatManager.MutexSocketServer.WaitOne();
                        if (ChatManager.SocketServer != null && ChatManager.SocketServer.Connected == true)
                        {
                                try
                                {
                                        ChatManager.SocketServer.Send(msg, size, SocketFlags.None);
                                }
                                catch (Exception e)
                                {
                                        Console.WriteLine(e.Message);
                                }
                        }
                        ChatManager.MutexSocketServer.ReleaseMutex();

                }
        }
}
