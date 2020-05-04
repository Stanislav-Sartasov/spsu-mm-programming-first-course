using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;


namespace P2PChatLibrary.Tests
{
    [TestClass]
    public class ChatConnectionTests
    {
        [TestMethod]
        public void CorrectUserInit()
        {
            var controller = new Mock<IChatController>();
            controller.Setup(n => n.GetUsername()).Returns("name");
            controller.Setup(p => p.GetLocalPort()).Returns(2000);
            ChatUI chat = new ChatUI(controller.Object);
            chat.UserInit();
            var actualInfo = chat.GetLocalUserInfo();
            Assert.AreEqual("name", actualInfo.Name);
            Assert.AreEqual(ClientUdp.GetLocalIPAddress(), IPAddress.Parse(actualInfo.Address));
            Assert.AreEqual(2000, actualInfo.Port);
        }


        [TestMethod]
        public void CorrectConnecting()
        {
            Task[] chatTasks = new Task[3];
            ChatUI[] chats = new ChatUI[3];
            var controller = new Mock<IChatController>();
            var actual = new List<List<UserInfo>>();
            var ts = new CancellationTokenSource[3];

            for (int i = 0; i < 3; i++)
            {
                controller = new Mock<IChatController>();
                controller.Setup(n => n.GetUsername()).Returns($"{i}");
                controller.Setup(p => p.GetLocalPort()).Returns(2001 + i);
                chats[i] = new ChatUI(controller.Object);
                ts[i] = new CancellationTokenSource();
                var ct = ts[i].Token;
                chatTasks[i] = new Task(chats[i].Start, ct);
                chatTasks[i].Start();
            }

            controller = new Mock<IChatController>();
            controller.Setup(n => n.GetUsername()).Returns("name");
            controller.Setup(p => p.GetLocalPort()).Returns(2000);
            int iteration = 0;
            controller.Setup(e => e.GetEndPoint())
                .Returns(() =>
                {
                    return new IPEndPoint(ClientUdp.GetLocalIPAddress(), 2001 + (iteration++));
                });
            controller.Setup(m => m.GetMessage())
                .Returns(() =>
            {
                if (iteration < 3)
                {
                    Thread.Sleep(50);
                    return "/connect";
                }
                else
                {
                    return "/exit";
                }
            });
            ChatUI chat = new ChatUI(controller.Object);
            chat.Start();
            Thread.Sleep(50);
            actual.AddRange(chats.Select(c => c.GetConnectedUsers()));
            for (int i = 0; i < 3; i++)
                ts[i].Cancel();
            foreach (var users in actual)
            {
                users.OrderByDescending(u => u.Port);
            }

            foreach (var (users1, users2) in from users1 in actual
                                             from users2 in actual
                                             select (users1, users2))
            {
                Assert.IsTrue(users1.Count == users2.Count && users2.Count == 3);
                foreach (var user in users1)
                {
                    Assert.IsTrue(users2.Contains(user));
                }
            }
        }
    }
}
