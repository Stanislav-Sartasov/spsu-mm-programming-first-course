using Chat.Clients;
using Chat.Manager;
using System;
using System.Text;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Application chat = new Application();
            Client client = new Client();
            chat.StartChating(client);
        }
    }
}
