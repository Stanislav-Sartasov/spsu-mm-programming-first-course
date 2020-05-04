using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using P2PChatLibrary;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PChat.Tests
{
    class ChatSendingTests
    {
        [TestMethod]
        public void CorrectMessageSending()
        {
            StringBuilder[] logs = new StringBuilder[3];

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
                int y = i;
                logs[i] = new StringBuilder();
                chats[i] = new ChatUI(controller.Object, s => logs[y].Append(s));
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
            string log = null;
            ChatUI chat = new ChatUI(controller.Object, s => log = s);
            chat.Start();
            Thread.Sleep(50);
            for (int i = 0; i < 3; i++)
            {
                ts[i].Cancel();
                string str = logs[i].ToString();
                Assert.IsTrue(str.Contains("name: hi"));
                Assert.IsTrue(str.Contains("name leaves us!"));
            }
        }
    }
}
