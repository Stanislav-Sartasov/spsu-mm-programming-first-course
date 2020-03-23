using System;
using System.Collections.Generic;
using HashTableLib;

namespace HashTable
{
    class Program
    {

        static void Main(string[] args)
        {
            Hashtable<int, string> table = new Hashtable<int, string>();
            string value;
            table.ContainsValue("sf");
            table.AddPair(1, "a");
            table.AddPair(1, "a");
            table.TryGetValue(1, out value);
            Console.WriteLine(value);
            Console.WriteLine(table.ContainsValue("a"));
        }
    }
}
