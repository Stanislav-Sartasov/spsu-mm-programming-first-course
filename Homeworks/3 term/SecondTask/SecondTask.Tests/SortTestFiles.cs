using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ArrayHandlerLib;

namespace SecondTask.Tests
{
	[TestClass]
	public class SortTestFiles
	{
		public string ActualFile { get; set; }
		public string ExpectedFile { get; set; }
		public int NumOfProcessors { get; set; }

		[TestInitialize]
		public void Init()
		{
			NumOfProcessors = 4;
			string path = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\ArraySources\";

			ArrayGeneration.GenerateTwoArrays(path);

			ExpectedFile = path + "sorted.dat";
			ActualFile = path + "my_sort.dat";
			var args = $"mpiexec -n {NumOfProcessors} SecondTask.exe \"{path + "unsorted.dat"}\" \"{ActualFile}\"";
			var MPIPath = path + @"..\..\bin\Debug\net5.0\";

			var cmd = new Process();
			cmd.StartInfo.FileName = "cmd.exe";
			cmd.StartInfo.RedirectStandardInput = true;
			cmd.StartInfo.RedirectStandardOutput = true;
			cmd.StartInfo.UseShellExecute = false;
			cmd.Start();

			cmd.StandardInput.WriteLine($"cd {MPIPath}");
			cmd.StandardInput.WriteLine(args);
			cmd.StandardInput.Flush();
			cmd.StandardInput.Close();
			cmd.WaitForExit();
		}

		[TestMethod]
		public void MPICheck()
		{
			Init();

			Assert.IsTrue(ArrayFileComparison.CompareTwoFileArrays(ExpectedFile, ActualFile));
		}
	}
}
