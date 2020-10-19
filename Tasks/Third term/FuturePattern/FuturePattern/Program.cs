using System;
using FuturePattern.Library;

namespace FuturePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Creator[] creators = new Creator[] { new CascadeCreator(), new ModifiedCascadeCreator() };
            int[] a = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] b = new int[3] { 1, 2, 3 };
            foreach (Creator creator in creators)
            {
                IVectorLengthComputer computer = creator.FactoryMethod();
                int res = computer.ComputeLength(a);
                Console.WriteLine(computer.Name() + " " + res.ToString());
            }
        }
    }
}
