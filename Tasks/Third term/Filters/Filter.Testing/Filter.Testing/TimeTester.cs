using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Filter.Testing
{
    class TimeTester
    {
        Thread[] threads;
        Client[] clients;

        public List<long> Test(int level, int iterations, string address, int height, int width)
        {
            List<long> result = new List<long>();

            clients = new Client[level];
            for (int i = 0; i < level; i++)
                clients[i] = new Client(address);
            threads = new Thread[level];
            for (int i = 0; i < level; i++)
            {
                var a = clients[i];
                threads[i] = new Thread(() => { a.Start(true, height, width); });
            }
            for (int i = 0; i < level; i++)
            {
                threads[i].Start();
            }

            Stopwatch timer = new Stopwatch();
            Client client = new Client(address);
            client.Start(false, height, width); // warm up
            for (int i = 0; i < iterations; i++)
            {
                timer.Restart();
                client.Start(false, height, width);
                timer.Stop();
                result.Add(timer.ElapsedMilliseconds);
            }
            client.Stop();
            for (int i = 0; i < level; i++)
            {
                clients[i].Stop();
            }
            for (int i = 0; i < level; i++)
            {
                threads[i].Join();
            }
            return result;
        }
    }
}
