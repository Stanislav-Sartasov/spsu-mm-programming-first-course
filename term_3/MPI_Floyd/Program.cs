using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;
using FloydAlgo;

namespace MPI_Floyd
{
    class Program
    {
        static void Main(string[] args)
        {          
            using (var env = new MPI.Environment(ref args))
            {
                Intracommunicator world = Communicator.world;
                int rank = world.Rank;
                
                int processorNumber = MPI.Communicator.world.Size;
                int myRank = MPI.Communicator.world.Rank;
                if (args.Length != 2)
                    return;

                if (myRank == 0)
                {
                    Matrix matrix = FileManager.FileToMatrix(args[0]);
                    Console.WriteLine($"about file: number of vert = {matrix.Height}");
                    for (long i = 0; i < matrix.Height; ++i)
                    {
                        for (long j = 0; j < matrix.Height; ++j)
                            Console.Write($"{matrix.Array[i, j]} ");
                        Console.WriteLine();
                    }
                    Console.WriteLine("////////////////////////////////////////////////////");
                    long oneProcessSize = matrix.Height / (processorNumber - 1);
                    Console.WriteLine($"procesorNumber {processorNumber}");
                    for (int i = 1; i < processorNumber; ++i)
                    {
                        Console.WriteLine($"talk about block {i}");
                        long size = oneProcessSize;
                        if (i == processorNumber - 1)
                        {
                            size += matrix.Height % (processorNumber - 1);
                            Console.WriteLine($"last block: bigger on {matrix.Height % (processorNumber - 1)}");
                        }
                        long[,] myMatrix = new long[size, matrix.Height];
                        for (long a = 0; a < oneProcessSize; ++a)
                        {
                            for (long b = 0; b < matrix.Height; ++b)
                            {
                                myMatrix[a, b] = matrix.Array[(i - 1) * oneProcessSize + a, b];
                                Console.Write($"{myMatrix[a, b]} ");
                            }
                            Console.WriteLine();
                        }
                        if ((i == processorNumber - 1) && (oneProcessSize < size))
                        {
                            Console.WriteLine("addition");
                            for (long a = oneProcessSize; a < size; ++a)
                            {
                                for (long b = 0; b < matrix.Height; ++b)
                                {
                                    myMatrix[a, b] = matrix.Array[(i - 1) * oneProcessSize + a, b];
                                    Console.Write($"{myMatrix[a, b]} ");
                                }
                                Console.WriteLine();
                            }
                        }
                        Matrix matrixForProcess = new Matrix(size, matrix.Height, myMatrix);
                        MPI.Communicator.world.Send<Matrix>(matrixForProcess, i, 0);
                    }
                    long processDone = 1;
                    while (processDone != processorNumber)
                    {
                        MPI.Status status = MPI.Communicator.world.Probe(MPI.Communicator.anySource, MPI.Communicator.anyTag);
                        //Console.WriteLine($"Rank {myRank}, status: {status.Source} {status.Tag}");
                        if (status.Tag == 3)
                        {
                            _ = MPI.Communicator.world.Receive<bool>(status.Source, 3);
                            processDone++;
                        }
                        if (status.Tag == 2)
                        {
                            Triple<long, long> value = MPI.Communicator.world.Receive<Triple<long, long>>(status.Source, 2);
                            matrix.Array[oneProcessSize*(status.Source - 1) + value.first, value.second] = value.value;
                        }
                        if (status.Tag == 1)
                        {
                            Triple<long, bool> value = MPI.Communicator.world.Receive<Triple<long, bool>>(status.Source, 1);
                            MPI.Communicator.world.Send <long>(matrix.Array[value.first, value.second], status.Source, 1);
                            //Triple<long, long> valueSecond = MPI.Communicator.world.Receive<Triple<long, long>>(status.Source, 2);
                            //Console.WriteLine($"change: source: {status.Source}, size: {oneProcessSize}, index: {valueSecond.first}");
                            //matrix.Array[oneProcessSize * (status.Source - 1) + valueSecond.first, valueSecond.second] = valueSecond.value;
                        }
                    }
                    Console.WriteLine("////////////////////////////////////////////////////");
                    for (long i = 0; i < matrix.Height; ++i)
                    {
                        for (long j = 0; j < matrix.Height; ++j)
                            Console.Write($"{matrix.Array[i, j]} ");
                        Console.WriteLine();
                    }
                    FileManager.MatrixToFile(args[1], matrix);

                }
                else
                {
                    Matrix m = MPI.Communicator.world.Receive<Matrix>(0, 0);
                    long oneprocess = m.Width / (processorNumber - 1);
                    long bottom = ((long)(myRank) - 1) * oneprocess;
                    long top = bottom + oneprocess - 1;
                    if (myRank == processorNumber - 1)
                    {
                        top = m.Width - 1;
                    }
                    

                    for (long i = 0; i < m.Height; ++i)
                    {
                        for (long j = 0; j < m.Width; ++j)
                        {
                            for (long k = 0; k < m.Width; ++k)
                            {
                                long valueKJ = 0;
                                if (k >= bottom && k <= top)
                                {
                                    valueKJ = m.Array[k - bottom, j];
                                }
                                else
                                {
                                    MPI.Communicator.world.Send<Triple<long, bool>>(new Triple<long, bool>(k, j, true), 0, 1);
                                    valueKJ = MPI.Communicator.world.Receive<long>(0, 1); //array [k , j]
                                }
                                if ( ((m.Array[i, j] > (m.Array[i, k] + valueKJ)) &&
                                        (m.Array[i, k] >= 0) &&
                                         (valueKJ >= 0)) ||
                                    ((m.Array[i, k] >= 0) &&
                                    (valueKJ >= 0) &&
                                    (m.Array[i, j] < 0)))
                                {
                                    m.Array[i, j] = m.Array[i, k] + valueKJ;
                                    MPI.Communicator.world.Send<Triple<long, long>>(new Triple<long, long>(i, j, m.Array[i,j]), 0, 2);
                                }
                                //MPI.Communicator.world.Send<Triple<long, long>>(new Triple<long, long>(i, j, m.Array[i, j]), 0, 2);
                                //i->k
                                //k->j 

                            }
                        }
                    }
                    MPI.Communicator.world.Send<bool>(true, 0, 3);
                }
            }

        }
    }
}
