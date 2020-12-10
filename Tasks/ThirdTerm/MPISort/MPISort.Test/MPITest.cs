using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FileManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPISort.Test
{
    [TestClass]
    public class MPITest
    {
        string inputPath = Directory.GetCurrentDirectory() + "\\dat.in";
        string outputPath = Directory.GetCurrentDirectory() + "\\dat.out";

        [TestMethod]
        public void TestMethod()
        {
            List<int> unsortedList = new List<int>() { 4, 534, 63, 46, 23, 24, 6, 345, 63, 5, 235, 24, 634, 6, 356, 23, 23, 423, 2, 5, 476, 7, 6, 543, 45, 346 };

            IOManager.WriteListToFile(inputPath, unsortedList);

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine($"mpiexec -n 8 {Path.GetFullPath(Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\" + "MPISort\\bin\\Debug\\netcoreapp3.1\\MPISort.exe")} {inputPath} {outputPath}");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            List<int> sortedList = IOManager.ReadFileToList(outputPath);
            unsortedList.Sort();

            for (int i = 0; i < unsortedList.Count; i++)
                Assert.AreEqual(unsortedList[i], sortedList[i]);
            
            File.Delete(inputPath);
            File.Delete(outputPath);
        }
    }
}
