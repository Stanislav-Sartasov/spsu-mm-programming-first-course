using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Diagnostics;

namespace MPIUnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void FloydTest()
        {
            string pathMPI = "..\\..\\..\\bin\\Debug";
            string program = "MPI_Floyd.exe";
            string fileIn = "data.in";
            string fileOut = "data.out";
            int processNumber = 4;
            string command = $"mpiexec -n {processNumber} {program} {fileIn} {fileOut}";
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine($"cd {pathMPI}");
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            StreamReader test = new StreamReader(Directory.GetCurrentDirectory() + "\\" + pathMPI + "\\" + fileOut);
            string testStr = test.ReadToEnd();
            StreamReader sample = new StreamReader(Directory.GetCurrentDirectory() + "\\" + pathMPI + "\\" + "sample.txt");
            string sampleStr = sample.ReadToEnd();
            test.Close();
            sample.Close();
            Assert.AreEqual(sampleStr, testStr);
        }
    }
}
