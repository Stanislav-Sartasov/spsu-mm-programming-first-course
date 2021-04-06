using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloydAlgo
{
    public static class FileManager
    {
        public static Matrix FileToMatrix(string path)
        {
            string line;
            long[,] matrix;
            long numberOfVert;
            try
            {
                string[] parsedLine;
                StreamReader sr = new StreamReader(path);
                line = sr.ReadLine();
                numberOfVert = long.Parse(line);
                //Console.WriteLine($"{long.Parse(line)}");
                matrix = new long[numberOfVert, numberOfVert];
                for (long i = 0; i < numberOfVert; ++i)
                    for (long j = 0; j < numberOfVert; ++j)
                    {
                        matrix[i, j] = -1;
                        //Console.WriteLine($"{i}--{j}");
                    }

                while (line != null)
                {
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        parsedLine = line.Split(' ');
                       // Console.WriteLine($"{line}--->{long.Parse(parsedLine[0]) - 1} -- {long.Parse(parsedLine[1]) - 1} -- {long.Parse(parsedLine[2])}");
                        matrix[long.Parse(parsedLine[0]), long.Parse(parsedLine[1])] = long.Parse(parsedLine[2]);
                    }
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("error in file read\nException: " + e.Message);
                throw new Exception("error in read file");
            }
            return new Matrix(numberOfVert, matrix);
        }

        public static void MatrixToFile(string path, Matrix matrixToWrite)
        {
            StreamWriter writer = new StreamWriter(path);

            writer.WriteLine($"{matrixToWrite.Height}");
            for (long i = 0; i < matrixToWrite.Height; ++i)
                for (long j = 0; j < matrixToWrite.Height; ++j)
                    writer.WriteLine($"{i} {j} {matrixToWrite.Array[i, j]}");
            writer.Close();
            writer.Dispose();
            return;
        }
    }
}
