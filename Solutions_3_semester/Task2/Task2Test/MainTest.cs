using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Task2Test
{
	public class Tests
	{
		public Tests()
		{
			System.IO.File.WriteAllText("test_graph_500.txt", Properties.Resources.test_graph_500);
			System.IO.File.WriteAllText("test_graph_5435.txt", Properties.Resources.test_graph_5435);
			System.IO.File.WriteAllBytes("Task2.exe", Properties.Resources.Task2);
		}
		void BruteForce(string inputPath, string outputPath)
		{
			StreamReader streamReader = new StreamReader(inputPath);
			StreamWriter streamWriter = new StreamWriter(outputPath);

			int size = int.Parse(streamReader.ReadLine());
			int[,] graph = new int[size, size];
			for (int i = 0; i < size; i++)
				for (int j = 0; j < size; j++)
					if (i != j)
						graph[i, j] = -1;
					else
						graph[i, j] = 0;

			while (!streamReader.EndOfStream)
			{
				List<int> edge = new List<int>();
				foreach (var s in streamReader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries))
					edge.Add(int.Parse(s));
				graph[edge[0], edge[1]] = edge[2];
				graph[edge[1], edge[0]] = edge[2];
			}

			for (int k = 0; k < size; k++)
				for (int i = 0; i < size; i++)
					for (int j = 0; j < size; j++)
						if (graph[i, k] != -1 && graph[k, j] != -1 && k != i && k != j && i != j)
						{
							int newVal = graph[i, k] + graph[k, j];
							if (graph[i, j] == -1 || newVal < graph[i, j])
							{
								graph[i, j] = newVal;
								graph[j, i] = newVal;
							}
						}

			streamWriter.WriteLine(size);

			for (int i = 0; i < size; i++)
				for (int j = i + 1; j < size; j++)
					if (graph[i, j] != -1)
						streamWriter.WriteLine($"{i} {j} {graph[i, j]}");

			streamReader.Close();
			streamWriter.Close();
		}

		[Test]
		public void CorrectnessTest()
		{
			const string testFileName = "test_graph_500";

			BruteForce($"{testFileName}.txt", $"{testFileName}_brute_force.txt");

			Process cmd = new Process();
			cmd.StartInfo.FileName = "cmd.exe";
			cmd.StartInfo.RedirectStandardInput = true;
			cmd.StartInfo.RedirectStandardOutput = true;
			cmd.StartInfo.UseShellExecute = false;
			cmd.Start();
			cmd.StandardInput.WriteLine($"cd {Environment.CurrentDirectory}");
			cmd.StandardInput.WriteLine($"mpiexec -n 4 Task2.exe {testFileName}.txt {testFileName}_mpi.txt");
			cmd.StandardInput.Flush();
			cmd.StandardInput.Close();
			cmd.WaitForExit();

			Assert.AreEqual(File.ReadAllText($"{testFileName}_brute_force.txt"), File.ReadAllText($"{testFileName}_mpi.txt"));
		}

		[Test]
		public void BruteForceSpeedTestGraph500()
		{
			const string testFileName = "test_graph_500";

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			BruteForce($"{testFileName}.txt", $"{testFileName}_brute_force.txt");
			stopwatch.Stop();

			Console.WriteLine($"completed in {stopwatch.ElapsedMilliseconds}ms");
		}

		[Test]
		public void MPISpeedTestGraph500()
		{
			const string testFileName = "test_graph_500";

			Process cmd = new Process();
			cmd.StartInfo.FileName = "cmd.exe";
			cmd.StartInfo.RedirectStandardInput = true;
			cmd.StartInfo.RedirectStandardOutput = true;
			cmd.StartInfo.UseShellExecute = false;
			cmd.Start();
			cmd.StandardInput.WriteLine($"cd {Environment.CurrentDirectory}");
			cmd.StandardInput.WriteLine($"mpiexec -n 8 Task2.exe {testFileName}.txt {testFileName}_mpi.txt");

			bool stop = false;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			cmd.StandardInput.Flush();
			cmd.StandardInput.Close();
			Task reader = Task.Run(() =>
			{
				while (!stop)
				{
					string str = cmd.StandardOutput.ReadLine();
					if (str != null && str != "")
						Console.WriteLine(str);
					Thread.Sleep(100);
				}
			});
			cmd.WaitForExit();
			stopwatch.Stop();

			stop = true;
			reader.Wait();

			Console.WriteLine($"completed in {stopwatch.ElapsedMilliseconds}ms");
		}

		[Test]
		public void MPISpeedTestGraphBig()
		{
			const string testFileName = "test_graph_5435";

			Process cmd = new Process();
			cmd.StartInfo.FileName = "cmd.exe";
			cmd.StartInfo.RedirectStandardInput = true;
			cmd.StartInfo.RedirectStandardOutput = true;
			cmd.StartInfo.UseShellExecute = false;
			cmd.Start();
			cmd.StandardInput.WriteLine($"cd {Environment.CurrentDirectory}");
			cmd.StandardInput.WriteLine($"mpiexec -n 8 Task2.exe {testFileName}.txt {testFileName}_mpi.txt");

			bool stop = false;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			cmd.StandardInput.Flush();
			cmd.StandardInput.Close();
			Task reader = Task.Run(() =>
			{
				while (!stop)
				{
					string str = cmd.StandardOutput.ReadLine();
					if (str != null && str != "")
						Console.WriteLine(str);
					Thread.Sleep(100);
				}
			});
			cmd.WaitForExit();
			stopwatch.Stop();

			stop = true;
			reader.Wait();

			Console.WriteLine($"completed in {stopwatch.ElapsedMilliseconds}ms");
		}
	}
}