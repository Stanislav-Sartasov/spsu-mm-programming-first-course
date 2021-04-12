using System;
using System.Collections.Generic;
using MPI;
using ArrayHandlerLib;

namespace SecondTask
{
	class RSQSort
	{
		static void Main(string[] args)
		{
			using (new MPI.Environment(ref args))
			{
				var comm = Communicator.world;
				try
				{
					var arr = new List<int>();

					if (args.Length != 2)
					{
						if (comm.Rank == 0)
						{
							throw new Exception("Error in input format (mpiexec -n <procNum> SecondTask.exe <input.txt> <output.txt>), please try again.");
						}
						return;
					}

					int sizeOfArray = 0;
					if (comm.Rank == 0)
					{
						arr = TextFilesLib.ReadArray(args[0]);

						sizeOfArray = arr.Count;

						if (sizeOfArray < comm.Size * comm.Size)
						{
							SingleQuickSort.QuickSort(arr, 0, arr.Count - 1);

							TextFilesLib.WriteArray(args[1], arr);

							arr.Clear();
							return;
						}
						else
						{
							for (int i = 1; i < comm.Size; i++)
							{
								comm.Send(arr, i, 0);
							}
						}
					}
					else
					{
						arr = comm.Receive<List<int>>(0, 0);
						sizeOfArray = arr.Count;
					}

					var nodeArr = new List<int>();
					int procSize = sizeOfArray / comm.Size;

					if (comm.Size - comm.Rank == 1)
					{
						for (int i = procSize * comm.Rank; i < sizeOfArray; i++)
						{
							nodeArr.Add(arr[i]);
						}
					}
					else
					{
						for (int i = procSize * comm.Rank; i < procSize * (comm.Rank + 1); i++)
						{
							nodeArr.Add(arr[i]);
						}
					}
					arr.Clear();

					SingleQuickSort.QuickSort(nodeArr, 0, nodeArr.Count - 1);

					comm.Barrier();

					var sepArr = new List<int>();
					int firstStep = sizeOfArray / (comm.Size * comm.Size);
					if (comm.Rank == 0)
					{
						for (int i = 0; i < firstStep * comm.Size; i += firstStep)
						{
							sepArr.Add(nodeArr[i]);
							for (int j = 1; j < comm.Size; j++)
							{
								sepArr.Add(comm.Receive<int>(j, 0));
							}
						}

						SingleQuickSort.QuickSort(sepArr, 0, sepArr.Count - 1);
					}
					else
					{
						for (int i = 0; i < firstStep * comm.Size; i += firstStep)
						{
							comm.Send(nodeArr[i], 0, 0);
						}
					}

					var pivArr = new List<int>();
					if (comm.Rank == 0)
					{
						int i = comm.Size + comm.Size / 2 - 1;
						for (; i < comm.Size * comm.Size + comm.Size / 2 - 1; i += comm.Size)
						{
							pivArr.Add(sepArr[i]);
						}
					}

					if (comm.Rank == 0)
					{
						for (int i = 1; i < comm.Size; i++)
						{
							comm.Send(pivArr, i, 0);
						}
					}
					else
					{
						pivArr = comm.Receive<List<int>>(0, 0);
					}
					sepArr.Clear();

					comm.Barrier();

					var sendArr = new List<int>[comm.Size];
					for (int i = 0; i < comm.Size; i++)
					{
						sendArr[i] = new List<int>();
					}

					int k = 0, l = 0;
					while (k < nodeArr.Count)
					{
						if (l < pivArr.Count)
						{
							if (nodeArr[k] <= pivArr[l])
							{
								sendArr[l].Add(nodeArr[k]);
								k++;
							}
							else
							{
								l++;
							}
						}
						else
						{
							if (nodeArr[k] > pivArr[l - 1])
							{
								sendArr[l].Add(nodeArr[k]);
								k++;
							}
						}
					}
					pivArr.Clear();
					nodeArr.Clear();

					comm.Barrier();

					var temp = comm.Alltoall(sendArr);
					var finalSortArr = new List<int>();
					foreach (var t in temp)
					{
						finalSortArr.AddRange(t);
					}

					SingleQuickSort.QuickSort(finalSortArr, 0, finalSortArr.Count - 1);

					comm.Barrier();
					
					sendArr = comm.Gather(finalSortArr, 0);

					if (comm.Rank == 0)
					{
						foreach (var t in sendArr)
						{
							arr.AddRange(t);
						}

						TextFilesLib.WriteArray(args[1], arr);

						arr.Clear();
						Array.Clear(sendArr, 0, sendArr.Length);
					}
			
					finalSortArr.Clear();
					Array.Clear(temp, 0, temp.Length);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error has occured in [{comm.Rank}] node. The program has stopped working.\n" + ex.Message);
				}
			}
		}
	}
}