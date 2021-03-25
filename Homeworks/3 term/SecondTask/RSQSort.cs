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
			try
			{
				using (new MPI.Environment(ref args))
				{
					var comm = Communicator.world;

					var arr = new List<int>(); //исходный массив

					if (args.Length != 2)
					{
						if (comm.Rank == 0)
						{
							Console.WriteLine(args.Length);
							throw new Exception("Error in input format, please try again.");
						}
						else
						{
							return;
						}
					}

					arr = TextFilesLib.ReadArray(args[0]);

					comm.Barrier();

					if (comm.Size > arr.Count)
					{
						if (comm.Rank == 0)
						{
							SingleQuickSort.QuickSort(arr, 0, arr.Count - 1);

							TextFilesLib.WriteArray(args[1], arr);
						}
						return;
					}
					else
					{
						if (comm.Rank == 0)
						{
							for (int i = 1; i < comm.Size; i++)
							{
								comm.Send(arr, i, 0);
							}
						}
						else
						{
							arr = comm.Receive<List<int>>(0, 0);
						}
					}

					//comm.Barrier(); //??

					var nodeArr = new List<int>(); //массив элементов на конкретном процессорe
					int procSize = arr.Count / comm.Size;

					if (comm.Size - comm.Rank == 1)
					{
						for (int i = procSize * (comm.Rank + 1); i < arr.Count; i++)
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
					int size = arr.Count;
					arr.Clear();

					SingleQuickSort.QuickSort(nodeArr, 0, nodeArr.Count - 1);

					comm.Barrier(); //??

					var sepArr = new List<int>(); //массив элементов-разделителей
					int firstStep = arr.Count / (comm.Size * comm.Size);
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

					var pivArr = new List<int>(); //массив ведущих элементов
					if (comm.Rank == 0)
					{
						int i = comm.Size + comm.Size / 2 - 1;
						for (; i < comm.Size * comm.Size + comm.Size / 2 - 1; i += comm.Size)
						{
							pivArr.Add(sepArr[i]);
						}

					}

					comm.Barrier();

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

					var sendArr = new List<int>[comm.Size]; //массив для отправки

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
							if (nodeArr[k] > pivArr[k - 1])
							{
								sendArr[l].Add(nodeArr[k]);
								k++;
							}
						}
					}

					comm.Barrier();

					var finalSortArr = new List<int>(); //массив для последней сортировки
					for (int i = 0; i < comm.Size; i++)
					{
						if (comm.Rank == i)
						{
							for (int j = 0; j < comm.Size; j++)
							{
								if (comm.Rank != j)
								{
									finalSortArr.AddRange(comm.Receive<List<int>>(j, 0));
								}
								else
								{
									finalSortArr.AddRange(sendArr[comm.Rank]);
								}
							}
						}
						else
						{
							comm.Send(sendArr[i], i, comm.Rank);
						}
					}

					//comm.Barrier(); //??

					SingleQuickSort.QuickSort(finalSortArr, 0, finalSortArr.Count - 1);

					comm.Barrier();

					if (comm.Rank == 0)
					{
						arr.AddRange(finalSortArr);
						for (int i = 1; i < comm.Size; i++)
						{
							arr.AddRange(comm.Receive<List<int>>(0, i));
						}

						TextFilesLib.WriteArray(args[1], arr);
					}
					else
					{
						comm.Send(finalSortArr, 0, comm.Rank);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("The program has stopped working.\n" + ex.Message);
			}
		}
	}
}
