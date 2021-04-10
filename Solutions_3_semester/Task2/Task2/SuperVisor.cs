using System;
using System.Collections.Generic;
using System.IO;
using MPI;
using System.Collections;
using System.Text;

namespace Task2
{
	static class SuperVisor
	{
		public static void Start(Intracommunicator world, string[] args)
		{
			StreamWriter streamWriter = null;
			try
			{
				if (args.Length != 2)
					throw new Exception("two parameters expected: <file in path> <file out path>");

				int[][] graph = GraphGetter.GetFromPath(args[0]);
				streamWriter = new StreamWriter(args[1]);

				int coreCount = world.Size;
				int graphSize = graph.Length;

				int[] strCoordCore = new int[graphSize];

				List<int[]>[] workPlace = new List<int[]>[coreCount];
				List<int>[] decryption = new List<int>[coreCount];
				for (int i = 0; i < coreCount; i++)
				{
					workPlace[i] = new List<int[]>();
					decryption[i] = new List<int>();
				}

				int counter = 0;
				while (counter < graphSize)
					for (int core = 0; core < coreCount && counter < graphSize; core++)
					{
						workPlace[core].Add(graph[counter]);
						decryption[core].Add(counter);
						strCoordCore[counter] = core;
						counter++;
					}

				for (int i = 1; i < coreCount; i++)
				{
					world.Send(graphSize, i, 0);
					world.Send<int[]>(strCoordCore, i, 0);
					world.Send(workPlace[i], i, 0);
					world.Send(decryption[i], i, 0);
				}

				List<List<int[]>> result = new List<List<int[]>>
				{
					Core.Work(world, graphSize, strCoordCore, workPlace[0], decryption[0])
				};

				for (int i = 1; i < coreCount; i++)
					result.Add(world.Receive<List<int[]>>(i, 0));

				streamWriter.WriteLine(graphSize);

				for (int i = 0; i < graphSize; i++)
				{
					int ind = strCoordCore[i];
					int[] str = result[ind][0];
					result[ind].RemoveAt(0);
					for (int j = i + 1; j < graphSize; j++)
						if (str[j] != -1)
							streamWriter.WriteLine($"{i} {j} {str[j]}");
				}

				streamWriter.Close();
			}
			catch (Exception ex)
			{
				try
				{
					streamWriter.Close();
				}
				catch { }
				Console.WriteLine(ex);
				world.Abort(-1);
			}
		}
	}
}
