using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using P2PChat;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace UnitTestP2PChat
{
    [TestClass]
    public class UnitTest
    {
        public void BotFirst()
        {
            
            int portCount = 0;
            int stringCount = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetIp()).Returns(() =>
            {
                return "127.0.0.1";
            });
            mock.Setup(x => x.GetPort()).Returns(() =>
            {
                if (portCount == 0)
                {
                    ++portCount;
                    return 81;
                }
                if (portCount == 1)
                {
                    ++portCount;
                    return 80;
                }
                ++portCount;
                return portCount;
            });
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (stringCount == 0)
                {
                    stringCount++;
                    return "rderd";
                }
                if (stringCount == 5)
                    return "exit()";
                return "message from first bot - " + stringCount++;
            });
            mock.Setup(x => x.Show(It.IsAny<string>())).Callback(() => { return; });
            mock.Setup(x => x.ShowMessage(It.IsAny<RecivedMessage>())).Callback(() => { return; });
            mock.Setup(x => x.ShowSender(It.IsAny<RecivedMessage>())).Callback(() => { return; });
            mock.Setup(x => x.SystemShow(It.IsAny<int>(), It.IsAny<string>())).Callback((int y, string x) =>
            {
                Console.WriteLine("<1System>" + x + "<System>");
                return;
            });
            mock.Setup(x => x.SystemShow(It.IsAny<string>())).Callback((string x) => 
            {
                Console.WriteLine("<1System>" + x + "<System>");
                return; 
            });


            ChatManager manager = new ChatManager(mock.Object);
            manager.StartChatting();
        }
        public void BotSecond(int countMessages)
        {
            int portCount = 0;
            int stringCount = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetIp()).Returns(() =>
            {
                return "127.0.0.1";
            });
            mock.Setup(x => x.GetPort()).Returns(() =>
            {
                if (portCount == 0)
                {
                    ++portCount;
                    return 81;
                }
                if (portCount == 1)
                {
                    ++portCount;
                    return 80;
                }
                ++portCount;
                return portCount;
            });
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (stringCount == 0)
                {
                    stringCount++;
                    return "wait";
                }
                if (stringCount == countMessages)
                    return "exit()";
                //Thread.Sleep(1);////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                
                return "message from Second bot number: " + stringCount++;
            });
            mock.Setup(x => x.Show(It.IsAny<string>())).Callback((string x) => 
            {
                Console.WriteLine(x);
                return; 
            });
            mock.Setup(x => x.ShowMessage(It.IsAny<RecivedMessage>())).Callback(() => { return; });
            mock.Setup(x => x.ShowSender(It.IsAny<RecivedMessage>())).Callback(() => { return; });
            mock.Setup(x => x.SystemShow(It.IsAny<int>(), It.IsAny<string>())).Callback((int y, string x) =>
            {
                Console.WriteLine("<2System>" + x + "<System>");
                return;
            });
            mock.Setup(x => x.SystemShow(It.IsAny<string>())).Callback((string x) =>
            {
                Console.WriteLine("<2System>" + x + "<System>");
                return;
            });
            mock.Setup(x => x.GetPort()).Returns(() =>
            {
                if (portCount == 0)
                {
                    ++portCount;
                    return 80;
                }
                if (portCount == 1)
                {
                    ++portCount;
                    return 81;
                }
                ++portCount;
                return portCount;
            });

            ChatManager manager = new ChatManager(mock.Object);
            manager.StartChatting();
        }
        public void BotThird(int countMessages)
        {
            int portCount = 0;
            int stringCount = 0;
            var mock = new Mock<IInteraction>();
            mock.Setup(x => x.GetIp()).Returns(() =>
            {
                return "127.0.0.1";
            });
            mock.Setup(x => x.GetPort()).Returns(() =>
            {
                if (stringCount == 0)
                {
                    ++portCount;
                    return 80 + portCount;
                }
                if (stringCount == 1)
                {
                    ++portCount;
                    return 80;
                }
                ++portCount;
                return portCount;
            });
            mock.Setup(x => x.GetStr()).Returns(() =>
            {
                if (stringCount == 0)
                {
                    stringCount++;
                    return "rderd";
                }
                if (stringCount == countMessages)
                    return "exit()";
                return "message from third bot - " + stringCount++;
            });
            mock.Setup(x => x.Show(It.IsAny<string>())).Callback(() => { return; });
            mock.Setup(x => x.ShowMessage(It.IsAny<RecivedMessage>())).Callback(() => { return; });
            mock.Setup(x => x.ShowSender(It.IsAny<RecivedMessage>())).Callback(() => { return; });
            mock.Setup(x => x.SystemShow(It.IsAny<int>(), It.IsAny<string>())).Callback((int y, string x) =>
            {
                Console.WriteLine("<3System>" + x + "<System>");
                return;
            });
            mock.Setup(x => x.SystemShow(It.IsAny<string>())).Callback((string x) =>
            {
                Console.WriteLine("<3System>" + x + "<System>");
                return;
            });


            ChatManager manager = new ChatManager(mock.Object);
            manager.StartChatting();
        }
        [TestMethod]
        public void TestP2P()
        {
            Task firstBot = new Task(() => BotFirst());
            Task secondBot = new Task(() => BotSecond(9));
            secondBot.Start();
            Thread.Sleep(5000);
            firstBot.Start();
            Thread.Sleep(100);
            firstBot.Wait();
            secondBot.Wait();

            //Console.WriteLine($"\n end-----\ncalls of Input: {callsInput}\nMade bets: {callsBet}\nRounds: {callsReady - 1}");
        }
        [TestMethod]
        public void TestManyClients()
        {
            List<Task> taskList = new List<Task>();
            var server = new Task(() => BotSecond(9));
            for (int i = 0; i < 10; ++i)
            {
                taskList.Add(new Task(() => BotThird(100)));
            }
            server.Start();
            Thread.Sleep(5000);
            foreach (var task in taskList)
            {
                task.Start();
            }
            bool flag = false;
            while (!flag)
            {
                bool tmp = true;
                foreach (var task in taskList)
                {
                    tmp = tmp && task.IsCompleted;
                }
                flag = false || (tmp && server.IsCompleted); 
            }
            //Console.WriteLine($"\n end-----\ncalls of Input: {callsInput}\nMade bets: {callsBet}\nRounds: {callsReady - 1}");
        }
        [TestMethod]
        public void TestDropClients()
        {
            //Task firstBot = new Task(() => BotFirst());
            Task secondBot = new Task(() => BotSecond(30));
            Thread threadForKill = null;
            Task thirdBot = new Task(() =>
            {
                threadForKill = Thread.CurrentThread;
                BotThird(100);
            });
            secondBot.Start();
            Thread.Sleep(5000);
            //firstBot.Start();
            thirdBot.Start();
            Thread.Sleep(2000);
            threadForKill.Abort();
            //firstBot.Wait();
            secondBot.Wait();
        }
    }
}
