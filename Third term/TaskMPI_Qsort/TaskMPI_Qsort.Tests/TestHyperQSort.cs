using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaskMPI_Qsort.Tests
{
    [TestClass]
    public class TestHyperQSort
    {
        static string appPath = AppDomain.CurrentDomain.BaseDirectory
            .Replace("TaskMPI_Qsort.Tests", "TaskMPI_Qsort");
        static string exe = "TaskMPI_Qsort.exe";
        static string output = "out.dat";
        static string sorted = "sorted.dat";
        static string input = "input.dat";
        public void Initialize(int proc)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine($"cd {appPath}");
            cmd.StandardInput.WriteLine($"mpiexec.exe -n {proc} {exe} {input} {output}");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }
        public bool Compare(string firstFile, string secondFile)
        {
            string first = File.ReadAllText(firstFile);
            string second = File.ReadAllText(secondFile);

            if (first.Length != second.Length)
                return false;

            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                    return false;
            }
            return true;
        }
        [TestMethod]
        public void TestSort()
        {
            Initialize(8);
            Assert.IsTrue(Compare(Path.Combine(appPath, output), Path.Combine(appPath, sorted)));
        }
    }
}
