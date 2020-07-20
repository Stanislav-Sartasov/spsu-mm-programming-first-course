using Chat.Application;
using Chat.Client;

using System;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager chat = new Manager();
            User client = new User();
            chat.StartChating(client);
        }
    }
}
