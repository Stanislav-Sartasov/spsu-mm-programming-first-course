using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Filter.Testing
{
    class FailureTester
    {
        Thread[] threads;
        Client[] clients;

        public long Test(int users, string address, int height, int width)
        {
            Client client = new Client(address);
            client.Start(false, height, width); // warm up
            client.Stop();

            clients = new Client[users];
            for (int i = 0; i < users; i++)
                clients[i] = new Client(address);
            threads = new Thread[users];
            for (int i = 0; i < users; i++)
            {
                var a = clients[i];
                threads[i] = new Thread(() => { a.Start(false, height, width); });
            }

            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < users; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < users; i++)
            {
                threads[i].Join();
            }
            timer.Stop();

            for (int i = 0; i < users; i++)
            {
                
                clients[i].Stop();
            }

            return timer.ElapsedMilliseconds;
        }
    }
}
