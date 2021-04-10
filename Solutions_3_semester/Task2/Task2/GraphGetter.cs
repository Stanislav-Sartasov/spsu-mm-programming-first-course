using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Task2
{
	static class GraphGetter
	{
		public static int[][] GetFromPath(string path)
		{
			StreamReader streamReader = null;
			try
			{
				streamReader = new StreamReader(path);
				int size = int.Parse(streamReader.ReadLine());

				int[][] result = new int[size][];
				for (int i = 0; i < size; i++)
				{
					result[i] = new int[size];
					for (int j = 0; j < size; j++)
						if (i == j)
							result[i][j] = 0;
						else
							result[i][j] = -1;
				}

				while (!streamReader.EndOfStream)
				{
					string line = streamReader.ReadLine();
					try
					{
						string[] splitted = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

						if (splitted.Length != 3)
							throw new Exception($"three parameters expected");

						List<int> edge = new List<int>();
						foreach (var s in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
							edge.Add(int.Parse(s));

						if (result[edge[0]][edge[1]] != -1)
							throw new Exception("repeating edge");

						if (edge[0] == edge[1])
							throw new Exception("from vertex to same vertex edge");

						result[edge[0]][edge[1]] = edge[2];
						result[edge[1]][edge[0]] = edge[2];
					}
					catch (Exception ex)
					{
						throw new Exception("invalid file:\n" + ex.Message + $"\nin line: \"{line}\"");
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				try
				{
					streamReader.Close();
				}
				catch { }
				throw ex;
			}
		}
	}
}
