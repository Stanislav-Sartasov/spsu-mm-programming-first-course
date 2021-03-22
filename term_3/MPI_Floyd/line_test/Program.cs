using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloydAlgo;
using System.Diagnostics;
using System.IO;

namespace line_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix;
            try
            {
                matrix = FileManager.FileToMatrix("matrix.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("can not open file");
                return;
            }
            Console.WriteLine($"about file: number of vert = {matrix.Height}");
            for (long i = 0; i < matrix.Height; ++i)
            {
                for (long j = 0; j < matrix.Height; ++j)
                    Console.Write($"{matrix.Array[i, j]} ");
                Console.WriteLine();
            }
            Console.WriteLine("////////////////////////////////////////////////////");
                    
            for (long i = 0; i < matrix.Height; ++i)
            {
                for (long j = 0; j < matrix.Height; ++j)
                {
                    for (long k = 0; k < matrix.Height; ++k)
                    {
                        if  (((matrix.Array[i, j] > (matrix.Array[i, k] + matrix.Array[k, j])) &&
                            (matrix.Array[i, k] >= 0) &&
                            (matrix.Array[k, j] >= 0)) || 
                            ((matrix.Array[i, k] >= 0) &&
                            (matrix.Array[k, j] >= 0) &&
                            (matrix.Array[i, j] < 0)))
                        {
                            matrix.Array[i, j] = matrix.Array[i, k] + matrix.Array[k, j];
                        }
                    }
                }
            }            


            Console.WriteLine("////////////////////////////////////////////////////");
            for (long i = 0; i < matrix.Height; ++i)
            {
                for (long j = 0; j < matrix.Height; ++j)
                    Console.Write($"{matrix.Array[i, j]} ");
                Console.WriteLine();
            }
            FileManager.MatrixToFile("fileOUT.txt", matrix);
            Console.ReadKey();
        }
    }
}
