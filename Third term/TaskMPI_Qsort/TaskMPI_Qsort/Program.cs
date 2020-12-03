using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using MPI;
using Environment = System.Environment;

namespace TaskMPI_Qsort
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string[] data;
            int[] array;
            List<int> result;
            
            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect input. Enter 2 paths.");
                return;
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("File with path {0} does not exist.", args[0]);
                return;
            }
            using (StreamReader streamReader = new StreamReader(args[0]))
            {
                
                data = streamReader.ReadToEnd().Split(" ");
                array = new int[data.Length];

                for (int i = 0; i < data.Length; i++)
                    array[i] = int.Parse(data[i]);
                Sort<int>.HyperQuickSort(ref array, args);
                
                if (array == null)
                {
                    Console.WriteLine("-n must be a power of two");
                    return;
                }
                result = new List<int>(array);
                using (StreamWriter streamWriter = new StreamWriter(args[1]))
                {
                    streamWriter.Write(string.Join(" ", result));
                }
                data = null;
                array = null;
                result = null;
            }
            
        }

    }
}
