using System;
using System.Collections.Generic;
using System.Text;
using MPI;

namespace Task2
{
	static class Core
	{
		public static void Start(Intracommunicator world)
		{
			try
			{
				int size = world.Receive<int>(0, 0);
				int[] strCoordCore = world.Receive<int[]>(0, 0);
				List<int[]> workPlace = world.Receive<List<int[]>>(0, 0);
				List<int> decryption = world.Receive<List<int>>(0, 0);

				world.Send(Work(world, size, strCoordCore, workPlace, decryption), 0, 0);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				world.Abort(-1);
			}
		}

		public static List<int[]> Work(Intracommunicator world, int size, int[] strCoordCore, List<int[]> workPlace, List<int> decryption)
		{
			int counter = 0;
			int rank = world.Rank;
			int coreCount = world.Size;

			Console.WriteLine($"core[{rank}] started");

			for (int k = 0; k < size; k++)
			{
				if (rank == 0 && k % 100 == 0)
					Console.WriteLine($"{k} : {size}");

				int[] strK;

				if (strCoordCore[k] == rank)
				{
					for (int i = 0; i < coreCount; i++)
						if (i != rank)
							world.Send<int[]>(workPlace[counter], i, 0);
					strK = workPlace[counter];
					counter++;
				}
				else
					strK = world.Receive<int[]>(strCoordCore[k], 0);

				for (int i = 0; i < workPlace.Count; i++)
				{
					int ri = decryption[i];
					if (ri == k)
						continue;
					for (int j = 0; j < size; j++)
						if (ri != j && k != j && workPlace[i][k] != -1 && strK[j] != -1)
						{
							int newVal = workPlace[i][k] + strK[j];
							if (newVal < workPlace[i][j] || workPlace[i][j] == -1)
								workPlace[i][j] = newVal;
						}
				}
			}
			
			return workPlace;
		}
	}
}
