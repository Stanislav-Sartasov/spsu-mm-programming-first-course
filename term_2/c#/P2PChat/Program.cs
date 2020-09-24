using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PChat
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatManager manager = new ChatManager();
            manager.StartChatting();
            Console.WriteLine("press any key");
            Console.ReadKey();
        }
    }
}
