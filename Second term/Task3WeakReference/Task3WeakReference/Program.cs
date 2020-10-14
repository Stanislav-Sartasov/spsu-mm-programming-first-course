using System;
using System.Threading;

namespace Task3WeakReference
{
    class Program
    {
        static void Main(string[] args)
        {
            WeakHashMap<int, string> weakHashMap = new WeakHashMap<int, string>(6000);
            weakHashMap.Add(10, "xD");
            Thread.Sleep(3000);
            weakHashMap.Add(15, "B-)");
            weakHashMap.Search(10);
            Thread.Sleep(3000);
            GC.Collect();
            weakHashMap.Search(10);
            weakHashMap.Search(15);
            weakHashMap.Clear();
            Console.ReadKey();
        }
    }
}
