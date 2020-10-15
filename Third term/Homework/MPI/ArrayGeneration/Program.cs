using System;
using MPI;
using System.Linq;
using System.IO;

namespace ArrayGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args.FirstOrDefault() ??
                string.Format(string.Format("{0}test.txt", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"))));
            Console.WriteLine(path);
            var array = ArrayGenerator.GenerateArray(2000000);
            IOManager.WriteArray(path, array);
        }
    }
}
