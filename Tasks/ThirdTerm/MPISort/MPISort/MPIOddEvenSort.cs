using System;
using System.Collections.Generic;
using FileManager;

namespace MPISort
{
    class MPIOddEvenSort
    {
        static void Main(string[] args)
        {
            MPI.Environment.Run(ref args, intracommunicator =>
            {
                if (args.Length != 2)
                {
                    Console.WriteLine("Incorrect input.");
                    return;
                }

                int size = intracommunicator.Size;
                int rank = intracommunicator.Rank;
                List<int> block;
                List<int> recievedBlock = new List<int>();

                if (rank == 0)
                {
                    List<int> unsortedList = IOManager.ReadFileToList(args[0]);

                    if (size > unsortedList.Count || size == 1)
                    {
                        unsortedList.Sort();
                        IOManager.WriteListToFile(args[1], unsortedList);
                        return;
                    }

                    List<int>[] unsortedListSplitted = new List<int>[size];

                    for (int i = 0; i < unsortedList.Count; i++)
                    {
                        if (i % (unsortedList.Count / size) == 0 && i / (unsortedList.Count / size) < size)
                        {
                            unsortedListSplitted[i / (unsortedList.Count / size)] = new List<int>();
                        }

                        unsortedListSplitted[i / (unsortedList.Count / size) - (i / (unsortedList.Count / size) >= size ? 1 : 0)].Add(unsortedList[i]);
                    }
                    block = intracommunicator.Scatter(unsortedListSplitted, 0);
                }
                else
                {   
                    block = intracommunicator.Scatter<List<int>>(0);
                }

                intracommunicator.Barrier();

                int step;

                for (int i = 0; i < size; i++)
                {
                    step = i % 2;

                    intracommunicator.Barrier();

                    for (int j = 0; j < size / 2 - (step == 1 && size % 2 == 0 ? 1 : 0); j++)
                    {
                        if (2 * j + 1 + step == rank)
                        {
                            intracommunicator.Send(block, 2 * j + step, 1);
                            block = intracommunicator.Receive<List<int>>(2 * j + step, 2);
                        }
                        else if (2 * j + step == rank)
                        {
                            recievedBlock = intracommunicator.Receive<List<int>>(2 * j + 1 + step, 1);
                            recievedBlock.AddRange(block);
                            recievedBlock.Sort();
                            block = recievedBlock.GetRange(0, block.Count);
                            recievedBlock = recievedBlock.GetRange(block.Count, recievedBlock.Count - block.Count);
                            intracommunicator.Send(recievedBlock, 2 * j + 1 + step, 2);
                        }
                    }

                    intracommunicator.Barrier();
                }

                List<int>[] sortedListSplitted = intracommunicator.Gather(block, 0);

                intracommunicator.Barrier();

                if (rank == 0)
                {
                    List<int> sortedList = new List<int>();

                    for (int i = 0; i < sortedListSplitted.Length; i++)
                    {
                        sortedList.AddRange(sortedListSplitted[i]);
                    }

                    IOManager.WriteListToFile(args[1], sortedList);
                }
            });
        }
    }
}