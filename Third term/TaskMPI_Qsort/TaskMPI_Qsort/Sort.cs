using System;
using System.Collections.Generic;
using System.Text;
using MPI;

namespace TaskMPI_Qsort
{
    public static class Sort<T> where T: IComparable<T>
    {
        
        public static T[] Merge(T[] first, T[] second)
        {
            if (first == null)
                return first;
            else if (second == null)
                return second;
            T[] result = new T[first.Length + second.Length];
            Array.Copy(first, result, first.Length);
            Array.Copy(second, 0, result, first.Length, second.Length);
            Array.Sort(result);
            return result;
        }
        
        public static T[] Split(ref T[] array, T specific)
        {
            List<T> lower = new List<T>();
            List<T> upper = new List<T>();
            foreach (T t in array)
            {
                if (t.CompareTo(specific) < 0)
                    lower.Add(t);
                else
                    upper.Add(t);
            }
            array = lower.ToArray();
            return upper.ToArray();
        }

        private static void SendToLow(ref T[] array, T specific, Communicator world, int shift)
        {
            T[] upper = Split(ref array, specific);
            world.Send(array.Length, world.Rank - shift, 2);
            world.Send(array, world.Rank - shift, 0);
            Console.WriteLine($"{world.Rank} sent to {world.Rank - shift}.");
            array = upper;
        }
        private static void SendUpdates(ref T[] array, T specific, Communicator world, int shift)
        {
            int blockSize;
            world.Receive(world.Rank + shift, 2, out blockSize);
            T[] temp = new T[blockSize];
            world.Receive(world.Rank + shift, 0, ref temp);
            Console.WriteLine($"{world.Rank} received from {world.Rank + shift}.");
            T[] upper = Split(ref array, specific);

            world.Send(upper.Length, world.Rank + shift, 2);
            world.Send(upper, world.Rank + shift, 1);
            Console.WriteLine($"{world.Rank} sent to {world.Rank + shift}.");
            array = Merge(array, temp);
            Console.WriteLine($"{world.Rank} merged.");
        }
        public static void HyperQuickSort(ref T[] array, string[] args)
        {
            List<T> result = new List<T>();
            using (var env = new MPI.Environment(ref args))
            {
                Intracommunicator world = Communicator.world;
                uint procs = (uint)world.Size;
                if ((procs & (procs - 1)) != 0)
                {
                    array = null;
                    return;
                }

                if (world.Rank == 0)
                {
                    
                    for (int i = 1; i < world.Size; i++)
                    {
                        int[] part;
                        if (i < world.Size - 1)
                        {
                            part = new int[array.Length / world.Size];
                            Array.Copy(array, array.Length / world.Size * i, part, 0, array.Length / world.Size);
                        }
                        else
                        {
                            part = new int[array.Length / world.Size + array.Length % world.Size];
                            Array.Copy(array, array.Length / world.Size * i, part, 0,
                                array.Length / world.Size + array.Length % world.Size);
                        }
                        world.Send<int[]>(part, i, 0);
                    }
                    Array.Resize(ref array, array.Length / world.Size);

                }

                else
                {
                    array = world.Receive<T[]>(0, 0);
                }
                Array.Sort(array);
                int iterationCount = (int)Math.Ceiling(Math.Log2(world.Size));
                int shift = world.Size;
                T specific;
                for (int i = iterationCount; i > 0; i--)
                {
                    // List of processors with large numbers on the hyperplane
                    List<int> senders = new List<int>();
                    int sender = world.Size - 1;
                    do
                    {
                        senders.Add(sender);
                        sender -= shift;
                    } while (sender > 0);

                    shift = shift >> 1;
                    if ((world.Rank & shift) >> (i - 1) == 1)
                    {

                        int index = senders.IndexOf(world.Rank);
                        if (index != -1)
                        {
                            specific = array[array.Length / 2];
                            if (index == senders.Count - 1)
                            {
                                for (int j = senders[index] - 1; j >= 0; j--)
                                    world.Send(specific, j, 1);
                            }
                            else
                            {
                                for (int j = senders[index] - 1; j > senders[index + 1]; j--)
                                    world.Send(specific, j, 1);
                            }
                            SendToLow(ref array, specific, world, shift);
                        }
                        else
                        {
                            for (int j = senders.Count - 1; j >= 0; j--)
                            {
                                if (senders[j] > world.Rank)
                                {
                                    world.Receive(senders[j], 1, out specific);
                                    SendToLow(ref array, specific, world, shift);
                                    break;
                                }

                            }
                        }
                        int size;
                        world.Receive(world.Rank - shift, 2, out size);
                        T[] updates = new T[size];
                        world.Receive(world.Rank - shift, 1, ref updates);

                        Console.WriteLine($"{world.Rank} received updates from {world.Rank - shift}.");
                        array = Merge(updates, array);
                        Console.WriteLine($"{world.Rank} merged.");

                    }
                    else
                    {
                        for (int j = senders.Count - 1; j >= 0; j--)
                        {
                            if (senders[j] > world.Rank)
                            {
                                world.Receive(senders[j], 1, out specific);
                                SendUpdates(ref array, specific, world, shift);
                                break;
                            }
                        }


                    }
                }

                var full = world.Gather(array, 0);

                foreach (T[] block in full)
                {
                    foreach (T t in block)
                    {
                        result.Add(t);
                    }
                }
                array = result.ToArray();
            }

        }
    }       
}

