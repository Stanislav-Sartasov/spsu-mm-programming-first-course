using System;
using FuturePattern.Library;

namespace FuturePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            IVectorLengthComputer computer = Creator.Create(1);
            int[] a = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] b = new int[3] { 1, 2, 3 };
            int res = computer.ComputeLength(a);
            Console.WriteLine(res);
        }
    }
}
