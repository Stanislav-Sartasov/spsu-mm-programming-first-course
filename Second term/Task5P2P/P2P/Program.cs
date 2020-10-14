using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using P2P.Clients;


namespace P2P.Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            Chat chat = new Chat();
            chat.Registration(client);
            Console.ReadLine();

        }
    }
}
