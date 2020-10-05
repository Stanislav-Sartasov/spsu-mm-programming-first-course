using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PChat
{
    public class ChatManager
    {
        private Engine engine;
        private IInteraction inter;
        public ChatManager()
        {
            engine = new Engine();
            inter = new Interaction();
        }
        public ChatManager(IInteraction start)
        {
            inter = start;
            engine = new Engine(start);
        }
        public void StartChatting()
        {
            inter.Show("if you do not want to connect type 'wait' or type anything and then type ip addres and port of remote client as in the example (127.0.0.1<enter>8811<enter>): ");
            string control = inter.GetStr();
            if (control == "wait")
            {
                inter.Show("waiting for connection...\n");
                engine.WaitForConnect();
            }
            else
            {
                string ip = inter.GetIp();
                int port = inter.GetPort();
                engine.OpenConnection(new IPEndPoint(IPAddress.Parse(ip), port));
            }
            bool stop = false;
            inter.Show("if you want to get out from chating type 'exit()', if you want to connect to another people type 'connect()'\n");
            while (!stop)
            {
                Thread.Sleep(100);
                engine.GetMessage();
                engine.PostMessage();
                stop = engine.CloseConnection();
            }
            inter.Show("\nDisconnected successful\n");
        }

    }
}
