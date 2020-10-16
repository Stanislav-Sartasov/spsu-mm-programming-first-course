using System;
using System.Threading;

namespace Task9WF
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Manager manager = new Manager();
            manager.Start(new FormHandler(), new TCPListener(), new SocketManager(), new Messager());

            while(manager.Active)
                Thread.Sleep(1000);
        }
    }
}
