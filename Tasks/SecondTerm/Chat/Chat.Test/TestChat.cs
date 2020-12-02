using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserInterface;
using Users;

namespace Chat.Test
{
    [TestClass]
    public class TestChat
    {
        [TestMethod]
        public void TestConnection()
        {
            List<User> users = new List<User>() { new User(), new User(), new User()};
            List<EndPoint> usersEndPoint = new List<EndPoint>();

           for (int i = 0; i < users.Count; i++)
                foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        usersEndPoint.Add(new IPEndPoint(ip, i + 128));
                        break;
                    }

            Thread userOneServerThread = new Thread(new ThreadStart(users[0].StartServerPart));
            Thread userTwoServerThread = new Thread(new ThreadStart(users[1].StartServerPart));
            Thread userThreeServerThread = new Thread(new ThreadStart(users[2].StartServerPart));

            userOneServerThread.Start();
            userTwoServerThread.Start();
            userThreeServerThread.Start();

            users[0].myEndPoint = usersEndPoint[0];
            users[1].myEndPoint = usersEndPoint[1];
            users[2].myEndPoint = usersEndPoint[2];

            users[0].socket.Bind(users[0].myEndPoint);
            users[1].socket.Bind(users[1].myEndPoint);
            users[2].socket.Bind(users[2].myEndPoint);

            users[0].Connect(usersEndPoint[1]);
            users[2].Connect(usersEndPoint[1]);

            Thread.Sleep(1500);

            Assert.AreEqual(2, users[0].GetListOfConnectedUsers().Count);
            Assert.AreEqual(2, users[1].GetListOfConnectedUsers().Count);
            Assert.AreEqual(2, users[2].GetListOfConnectedUsers().Count);
        }
    }
}
