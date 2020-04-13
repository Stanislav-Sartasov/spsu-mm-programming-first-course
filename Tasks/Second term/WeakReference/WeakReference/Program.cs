using System;
using System.Threading;
using System.Threading.Tasks;
using DynamicArray;
using System.Collections.Generic;


namespace WeakReference
{
    class Program
    {
        static WeakDynamicArray<String> dynamicArray = new WeakDynamicArray<String>(5000);
        static void TestWeakArray()
        {
            var o1 = new String("1");
            var o2 = new String("2");
            var o3 = new String("3");

            dynamicArray.AddToEnd(o1);
            dynamicArray.AddToEnd(o2);
            dynamicArray.AddToEnd(o3);
        }
        static void Main(string[] args)
        {
            TestWeakArray();
            Console.WriteLine("Start array");
            dynamicArray.PrintArray();

            Thread.Sleep(2000);
            GC.Collect();
            TestWeakArray();
            Console.WriteLine("Start array");
            dynamicArray.PrintArray();
            Thread.Sleep(4000);
            GC.Collect();
            TestWeakArray();
            Console.WriteLine("Start array");
            dynamicArray.PrintArray();

            Thread.Sleep(6000);
            GC.Collect();
            Console.WriteLine("Start array");
            dynamicArray.PrintArray();
        }
    }
}
