using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Text;

namespace P2PChatLibrary.Tests
{
    [TestClass]
    public class ChatConnectionTests
    {
        List<List<UserInfo>> actual;
        StringBuilder[] logs;
       
        [TestInitialize]
        public void TestInit()
        {
            logs = new StringBuilder[3];
            actual = new List<List<UserInfo>>();

            Thread[] chatTasks = new Thread[3];
            ChatUI[] chats = new ChatUI[3];
            Mock<IChatController>[] controllers = new Mock<IChatController>[3];
            Mock<IChatController> mainController = new Mock<IChatController>();

            for (int i = 0; i < 3; i++)
            {
                int y = i;
                controllers[y] = new Mock<IChatController>();
                controllers[y].Setup(n => n.GetUsername()).Returns($"{y}");
                controllers[y].Setup(p => p.GetLocalPort()).Returns(2001 + y);
                logs[i] = new StringBuilder();
                chats[i] = new ChatUI(controllers[y].Object, s => logs[y].Append(s));
                chatTasks[y] = new Thread(() => chats[y].Start());
                chatTasks[y].Start();
            }

            mainController = new Mock<IChatController>();
            mainController.Setup(n => n.GetUsername()).Returns("name");
            mainController.Setup(p => p.GetLocalPort()).Returns(2000);
            int iteration = 0;
            mainController.Setup(e => e.GetEndPoint())
                .Returns(() =>
                {
                    return new IPEndPoint(ClientUdp.GetLocalIPAddress(), 2001 + (iteration++));
                });
            mainController.Setup(m => m.GetMessage())
                .Returns(() =>
                {
                    if (iteration < 3)
                    {
                        Thread.Sleep(50);
                        return "/connect";
                    }

                    else if (iteration == 3)
                    {
                        iteration++;
                        return "hi";
                    }
                    else
                    {
                        return "/exit";
                    }
                });
            ChatUI chat = new ChatUI(mainController.Object);
            chat.Start();
            Thread.Sleep(50);
            actual.AddRange(chats.Select(c => c.GetConnectedUsers()));
        }
        [TestMethod]
        public void CorrectConnection()
        {
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

            for (int i = 0; i < 3; i++)
            {
                string str = logs[i].ToString();
                Assert.IsTrue(str.Contains("name: hi"));
                Assert.IsTrue(str.Contains("name leaves us!"));
            }
        }
    }
}
