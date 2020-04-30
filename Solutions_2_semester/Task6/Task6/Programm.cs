using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using GetLib;
using System.IO;

namespace Task6
{
    public static class Programm
    {
        public static void Main() 
        {
            string way = Directory.GetCurrentDirectory();
            List<IFibonacci> objects = GetLib<IFibonacci>.FromDirectory(way);
            int num = 42;

            Console.WriteLine("Libs directory: " + way + "\n");
            for (int i = 0; i < objects.Count; i++)
                Console.WriteLine("Object[" + i.ToString() + "] said that Fibonacci(" + num.ToString() + ") = " + objects[i].GetFibonacci(num).ToString());
        }
    }
}
