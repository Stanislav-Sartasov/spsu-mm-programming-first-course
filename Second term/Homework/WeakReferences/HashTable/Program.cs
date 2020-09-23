using System;
using System.Threading;
using HashTableLib;

namespace HashTable
{
    class Program
    {

        static void Main(string[] args)
        {
            Hashtable<int, string> table = new Hashtable<int, string>(5000);
            table.AddPair(5, "5");
            table.AddPair(5, "5");
            table.AddPair(6, "6");
            table.AddPair(7, "7");
            table.AddPair(8, "8");
            table.TryGetValue(5, out string s);
            Console.WriteLine(s);
            table.DeleteByKey(5);
            table.Clear();
        }
    }
}
