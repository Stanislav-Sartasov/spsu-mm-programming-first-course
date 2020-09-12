using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2P.Clients;
using P2P.Chat;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace P2P.Tests
{
    [TestClass]
    public class TestClient
    {
        [TestMethod]
        public void TestConnection()
        {
            Client oliver = new Client();
            Client stepan = new Client();
            Client sasha = new Client();
            List<Client> clients = new List<Client>();

            clients.Add(oliver);
            clients.Add(stepan);
            clients.Add(sasha);
            int port = 10;
            foreach (Client client in clients)
            {
                IPEndPoint endPoint = null;
                
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        endPoint = new IPEndPoint(ip, port);
                    }
                }
                client.SetLocalPoint(endPoint);
                client.SetClient();
                port += 10;
            }
            oliver.Connect(stepan.GetLocalPoint());
            stepan.Connect(oliver.GetLocalPoint());
            Assert.IsTrue(oliver.GetClients().Contains(stepan.GetLocalPoint()));
            Assert.IsTrue(stepan.GetClients().Contains(oliver.GetLocalPoint()));

            sasha.Connect(oliver.GetLocalPoint());
            foreach (Client client in clients)
            {
                foreach (EndPoint endPoint in client.GetClients())
                {
                    Assert.IsTrue(client.GetClients().Contains(endPoint));
                }
            }
        }

    }
}
