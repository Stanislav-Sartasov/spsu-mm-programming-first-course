using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DekanatLibrary;
using System.Diagnostics;

namespace Dekanat
{
    class Program
    {
        private static IExamSystem examSys;
        private static Task[] users;
        private static Random random = new Random();
        private static void InitUsers(int userrsNumber)
        {
            users = new Task[userrsNumber];
            for (int i = 0; i < userrsNumber; ++i)
            {
                int present = random.Next(100);
                if (present == 0)
                    users[i] = new Task(() => examSys.Remove(random.Next(), random.Next()));
                else
                    if (present < 10)
                        users[i] = new Task(() => examSys.Add(random.Next(), random.Next()));
                    else
                        users[i] = new Task(() => examSys.Contains(random.Next(), random.Next()));
            }
        }
        private static long Work()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (Task user in users)
            {
                user.Start();
            }
            foreach (Task user in users)
            {
                user.Wait();
            }
            stopWatch.Stop();
            foreach (Task user in users)
            {
                user.Dispose();
            }
            return stopWatch.ElapsedMilliseconds;
        }
        static void Main(string[] args)
        {
            int size = 256;
            int requests = 65536;
            examSys = new HashTable(size);
            InitUsers(requests);
            Console.WriteLine($"striped hash table on startrd size {size}, request number {requests}, gets job done in {Work()} milliseconds");
            examSys = new CuckooHash(size);
            InitUsers(requests);
            Console.WriteLine($"CukooRefinable hash table on startrd size {size}, request number {requests}, gets job done in {Work()} milliseconds");
            Console.ReadKey();
        }
    }
}
