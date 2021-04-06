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
            string pathMPI = "..\\bin\\Debug\\";
            string pathTest = "\\..\\..";
            string program = "MPI_Floyd.exe";
            string fileIn = "data.in";
            string fileOut = "data.out";
            int processNumber = 4;
            string command = $"mpiexec -n {processNumber} {pathMPI + program} {fileIn} {fileOut}";
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine($"cd {Directory.GetCurrentDirectory()}{pathTest}");
            cmd.StandardInput.WriteLine(command);
            //while(! cmd.StandardOutput.EndOfStream)
                Console.WriteLine(cmd.StandardOutput.ReadLine());
            Console.WriteLine(cmd.StandardOutput.ReadLine());
            Console.WriteLine(cmd.StandardOutput.ReadLine());
            Console.WriteLine(cmd.StandardOutput.ReadLine());
            Console.WriteLine(cmd.StandardOutput.ReadLine());
            Console.WriteLine(cmd.StandardOutput.ReadLine());
            Console.WriteLine(cmd.StandardOutput.ReadLine());
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            
            StreamReader test = new StreamReader(Directory.GetCurrentDirectory() + pathTest + "\\" + fileOut);
            string testStr = test.ReadToEnd();
            StreamReader sample = new StreamReader(Directory.GetCurrentDirectory() + pathTest + "\\" + "sample.txt");
            string sampleStr = sample.ReadToEnd();
            test.Close();
            sample.Close();
            Assert.AreEqual(sampleStr, testStr);
        }
    }
}
