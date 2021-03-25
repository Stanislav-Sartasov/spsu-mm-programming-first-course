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
            int[] a = { 3, 4 };
            double result = vector.ComputeLength(a);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
