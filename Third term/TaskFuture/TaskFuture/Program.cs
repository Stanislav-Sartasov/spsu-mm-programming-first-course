using Models;
using System;

namespace TaskFuture
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = new int[] { 3, 4, 5, 6, 1, 200, 100 };
            CascadeModel cascadeModel = new CascadeModel();
            ModifiedCascade modifiedCascade = new ModifiedCascade();
            Console.WriteLine(cascadeModel.ComputeLength(a));
            Console.WriteLine(modifiedCascade.ComputeLength(a));
            Console.ReadLine();
        }
    }
}
