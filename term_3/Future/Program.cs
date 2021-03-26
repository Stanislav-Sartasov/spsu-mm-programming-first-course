using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FutureLibrary;
namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            IVectorLengthComputer vector = new Cascade();
            int[] a = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            double result = vector.ComputeLength(a);
            Console.WriteLine($"cascade schema: {result}");
            vector = new ModifiedCascade();
            result = vector.ComputeLength(a);
            Console.WriteLine($"modified schema: {result}");
            Console.ReadKey();
        }
    }
}
